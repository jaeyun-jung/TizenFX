/*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;

namespace ElmSharp
{
    public enum ListMode
    {
        Compress = 0,
        Scroll,
        Limit,
        Expand
    }

    public class ListItemEventArgs : EventArgs
    {
        public ListItem Item { get; set; }
        internal static ListItemEventArgs CreateFromSmartEvent(IntPtr data, IntPtr obj, IntPtr info)
        {
            ListItem item = ItemObject.GetItemByHandle(info) as ListItem;
            return new ListItemEventArgs() { Item = item };
        }
    }

    public class List : Layout
    {
        HashSet<ListItem> _children = new HashSet<ListItem>();
        SmartEvent<ListItemEventArgs> _selected;
        SmartEvent<ListItemEventArgs> _unselected;
        SmartEvent<ListItemEventArgs> _doubleClicked;
        SmartEvent<ListItemEventArgs> _longpressed;
        SmartEvent<ListItemEventArgs> _activated;

        public List(EvasObject parent) : base(parent)
        {
            _selected = new SmartEvent<ListItemEventArgs>(this, this.RealHandle, "selected", ListItemEventArgs.CreateFromSmartEvent);
            _unselected = new SmartEvent<ListItemEventArgs>(this, this.RealHandle, "unselected", ListItemEventArgs.CreateFromSmartEvent);
            _doubleClicked = new SmartEvent<ListItemEventArgs>(this, this.RealHandle, "clicked,double", ListItemEventArgs.CreateFromSmartEvent);
            _longpressed = new SmartEvent<ListItemEventArgs>(this, this.RealHandle, "longpressed", ListItemEventArgs.CreateFromSmartEvent);
            _activated = new SmartEvent<ListItemEventArgs>(this, this.RealHandle, "activated", ListItemEventArgs.CreateFromSmartEvent);
            _selected.On += (s, e) => { ItemSelected?.Invoke(this, e); };
            _unselected.On += (s, e) => { ItemUnselected?.Invoke(this, e); };
            _doubleClicked.On += (s, e) => { ItemDoubleClicked?.Invoke(this, e); };
            _longpressed.On += (s, e) => { ItemLongPressed?.Invoke(this, e); };
            _activated.On += (s, e) => { ItemActivated?.Invoke(this, e); };
        }

        public ListMode Mode
        {
            get
            {
                return (ListMode)Interop.Elementary.elm_list_mode_get(RealHandle);
            }
            set
            {
                Interop.Elementary.elm_list_mode_set(RealHandle, (Interop.Elementary.Elm_List_Mode)value);
            }
        }

        public ListItem SelectedItem
        {
            get
            {
                IntPtr item = Interop.Elementary.elm_list_selected_item_get(RealHandle);
                return ItemObject.GetItemByHandle(item) as ListItem;
            }
        }

        public event EventHandler<ListItemEventArgs> ItemSelected;
        public event EventHandler<ListItemEventArgs> ItemUnselected;
        public event EventHandler<ListItemEventArgs> ItemDoubleClicked;
        public event EventHandler<ListItemEventArgs> ItemLongPressed;
        public event EventHandler<ListItemEventArgs> ItemActivated;

        public void Update()
        {
            Interop.Elementary.elm_list_go(RealHandle);
        }

        public ListItem Append(string label)
        {
            return Append(label, null, null);
        }

        public ListItem Append(string label, EvasObject leftIcon, EvasObject rightIcon)
        {
            ListItem item = new ListItem(label, leftIcon, rightIcon);
            item.Handle = Interop.Elementary.elm_list_item_append(RealHandle, label, leftIcon, rightIcon, null, (IntPtr)item.Id);
            AddInternal(item);
            return item;
        }

        public ListItem Prepend(string label)
        {
            return Prepend(label, null, null);
        }

        public ListItem Prepend(string label, EvasObject leftIcon, EvasObject rigthIcon)
        {
            ListItem item = new ListItem(label, leftIcon, rigthIcon);
            item.Handle = Interop.Elementary.elm_list_item_prepend(RealHandle, label, leftIcon, rigthIcon, null, (IntPtr)item.Id);
            AddInternal(item);
            return item;
        }

        public void Clear()
        {
            Interop.Elementary.elm_list_clear(RealHandle);
            foreach (var item in _children)
            {
                item.Deleted -= Item_Deleted;
            }
            _children.Clear();
        }

        protected override IntPtr CreateHandle(EvasObject parent)
        {
            IntPtr handle = Interop.Elementary.elm_layout_add(parent.Handle);
            Interop.Elementary.elm_layout_theme_set(handle, "layout", "elm_widget", "default");

            RealHandle = Interop.Elementary.elm_list_add(handle);
            Interop.Elementary.elm_object_part_content_set(handle, "elm.swallow.content", RealHandle);

            return handle;
        }

        void AddInternal(ListItem item)
        {
            _children.Add(item);
            item.Deleted += Item_Deleted;
        }

        void Item_Deleted(object sender, EventArgs e)
        {
            _children.Remove((ListItem)sender);
        }
    }
}

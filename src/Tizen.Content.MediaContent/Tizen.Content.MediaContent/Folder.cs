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

namespace Tizen.Content.MediaContent
{
    /// <summary>
    /// Represents the folder information for media.
    /// </summary>
    /// <remarks>
    /// A <see cref="Folder"/> is used to organize media content files i.e. image, audio, video files,
    /// in the physical storage of the device.
    /// </remarks>
    public class Folder
    {
        internal Folder(IntPtr handle)
        {
            Id = InteropHelper.GetString(handle, Interop.Folder.GetFolderId);
            Path = InteropHelper.GetString(handle, Interop.Folder.GetPath);
            Name = InteropHelper.GetString(handle, Interop.Folder.GetName);

            StorageType = InteropHelper.GetValue<StorageType>(handle, Interop.Folder.GetStorageType);
            StorageId = InteropHelper.GetString(handle, Interop.Folder.GetStorageId);
        }

        internal static Folder FromHandle(IntPtr handle) => new Folder(handle);

        /// <summary>
        /// Gets the id of folder.
        /// </summary>
        /// <value>The unique id of folder.</value>
        public string Id { get; }

        /// <summary>
        /// Gets the path of folder.
        /// </summary>
        /// <value>The path of folder.</value>
        public string Path { get; }

        /// <summary>
        /// Gets the name of folder.
        /// </summary>
        /// <value>The name of folder.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the <see cref="StorageType"/> of the storage that the folder exists.
        /// </summary>
        /// <value>The <see cref="StorageType"/> of the storage that the folder exists.</value>
        public StorageType StorageType { get; }

        /// <summary>
        /// Gets the storage id of the storage that the folder exists.
        /// </summary>
        /// <value>The storage id of the storage that the folder exists.</value>
        public string StorageId { get; }

        /// <summary>
        /// Returns a string representation of the folder.
        /// </summary>
        /// <returns>A string representation of the current folder.</returns>
        public override string ToString() =>
            $"Id={Id}, Name={Name}, Path={Path}, StorageType={StorageType}, StorageId={StorageType}";
    }
}

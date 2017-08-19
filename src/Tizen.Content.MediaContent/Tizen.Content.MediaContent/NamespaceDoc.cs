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

/// <summary>
/// The Tizen.Content.MediaContent namespace provides types used in the entire content service.
/// The information about media items(i.e. image, audio and video) are managed in the content database
/// and operations that involve database require an active connection with the media content service.
/// During media scanning, Media content service extracts media information automatically. Media information
/// includes basic file info like path, size, modified time etc and some metadata like ID3 tag, EXIF,
/// thumbnail, etc. (thumbnail extracted only in Internal and SD card storage.
/// </summary>
/// <remarks>Media content service does not manage hidden files.</remarks>
namespace Tizen.Content.MediaContent { }
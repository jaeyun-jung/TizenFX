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

namespace Tizen.Sensor
{
    /// <summary>
    /// The OrientationSensor class is used for registering callbacks for the orientation sensor and getting the orientation data.
    /// </summary>
    /// <since_tizen> 3 </since_tizen>
    public sealed class OrientationSensor : Sensor
    {
        private static string OrientationSensorKey = "http://tizen.org/feature/sensor.tiltmeter";

        private event EventHandler<SensorAccuracyChangedEventArgs> _accuracyChanged;
        /// <summary>
        /// Gets the azimuth component of the orientation.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <value> Azimuth </value>
        public float Azimuth { get; private set; } = float.MinValue;

        /// <summary>
        /// Gets the pitch component of the orientation.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <value> Pitch </value>
        public float Pitch { get; private set; } = float.MinValue;

        /// <summary>
        /// Gets the roll component of the orientation.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <value> Roll </value>
        public float Roll { get; private set; } = float.MinValue;

        /// <summary>
        /// Returns true or false based on whether the orientation sensor is supported by the device.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <value><c>true</c> if supported; otherwise <c>false</c>.</value>
        public static bool IsSupported
        {
            get
            {
                Log.Info(Globals.LogTag, "Checking if the OrientationSensor is supported");
                return CheckIfSupported(SensorType.OrientationSensor, OrientationSensorKey);
            }
        }

        /// <summary>
        /// Returns the number of orientation sensors available on the device.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <value> The count of orientation sensors. </value>
        public static int Count
        {
            get
            {
                Log.Info(Globals.LogTag, "Getting the count of orientation sensors");
                return GetCount();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tizen.Sensor.OrientationSensor"/> class.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        /// <feature>http://tizen.org/feature/sensor.tiltmeter</feature>
        /// <exception cref="ArgumentException">Thrown when an invalid argument is used.</exception>
        /// <exception cref="NotSupportedException">Thrown when the sensor is not supported.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the operation is invalid for the current state.</exception>
        /// <param name='index'>
        /// Index. Default value for this is 0. Index refers to a particular orientation sensor in case of multiple sensors.
        /// </param>
        public OrientationSensor(uint index = 0) : base(index)
        {
            Log.Info(Globals.LogTag, "Creating OrientationSensor object");
        }

        internal override SensorType GetSensorType()
        {
            return SensorType.OrientationSensor;
        }

        /// <summary>
        /// An event handler for storing the callback functions for the event corresponding to the change in the orientation sensor data.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>

        public event EventHandler<OrientationSensorDataUpdatedEventArgs> DataUpdated;

        /// <summary>
        /// An event handler for accuracy changed events.
        /// </summary>
        /// <since_tizen> 3 </since_tizen>
        public event EventHandler<SensorAccuracyChangedEventArgs> AccuracyChanged
        {
            add
            {
                if (_accuracyChanged == null)
                {
                    AccuracyListenStart();
                }
                _accuracyChanged += value;
            }
            remove
            {
                _accuracyChanged -= value;
                if (_accuracyChanged == null)
                {
                    AccuracyListenStop();
                }
            }
        }

        private static int GetCount()
        {
            IntPtr list;
            int count;
            int error = Interop.SensorManager.GetSensorList(SensorType.OrientationSensor, out list, out count);
            if (error != (int)SensorError.None)
            {
                Log.Error(Globals.LogTag, "Error getting sensor list for orientation");
                count = 0;
            }
            else
                Interop.Libc.Free(list);
            return count;
        }

        private static Interop.SensorListener.SensorEventCallback _callback;

        internal override void EventListenStart()
        {
            _callback = (IntPtr sensorHandle, IntPtr eventPtr, IntPtr data) => {
                Interop.SensorEventStruct sensorData = Interop.IntPtrToEventStruct(eventPtr);

                TimeSpan = new TimeSpan((Int64)sensorData.timestamp);
                Azimuth = sensorData.values[0];
                Pitch = sensorData.values[1];
                Roll = sensorData.values[2];

                DataUpdated?.Invoke(this, new OrientationSensorDataUpdatedEventArgs(sensorData.values));
            };

            int error = Interop.SensorListener.SetEventCallback(ListenerHandle, Interval, _callback, IntPtr.Zero);
            if (error != (int)SensorError.None)
            {
                Log.Error(Globals.LogTag, "Error setting event callback for orientation sensor");
                throw SensorErrorFactory.CheckAndThrowException(error, "Unable to set event callback for orientation");
            }
        }

        internal override void EventListenStop()
        {
            int error = Interop.SensorListener.UnsetEventCallback(ListenerHandle);
            if (error != (int)SensorError.None)
            {
                Log.Error(Globals.LogTag, "Error unsetting event callback for orientation sensor");
                throw SensorErrorFactory.CheckAndThrowException(error, "Unable to unset event callback for orientation");
            }
        }

        private static Interop.SensorListener.SensorAccuracyCallback _accuracyCallback;

        private void AccuracyListenStart()
        {
            _accuracyCallback = (IntPtr sensorHandle, UInt64 timestamp, SensorDataAccuracy accuracy, IntPtr data) => {
                TimeSpan = new TimeSpan((Int64)timestamp);
                _accuracyChanged?.Invoke(this, new SensorAccuracyChangedEventArgs(new TimeSpan((Int64)timestamp), accuracy));
            };

            int error = Interop.SensorListener.SetAccuracyCallback(ListenerHandle, _accuracyCallback, IntPtr.Zero);
            if (error != (int)SensorError.None)
            {
                Log.Error(Globals.LogTag, "Error setting accuracy event callback for orientation sensor");
                throw SensorErrorFactory.CheckAndThrowException(error, "Unable to set accuracy accuracy event callback for orientation");
            }
        }

        private void AccuracyListenStop()
        {
            int error = Interop.SensorListener.UnsetAccuracyCallback(ListenerHandle);
            if (error != (int)SensorError.None)
            {
                Log.Error(Globals.LogTag, "Error unsetting event callback for orientation sensor");
                throw SensorErrorFactory.CheckAndThrowException(error, "Unable to unset accuracy event callback for orientation");
            }
        }
    }
}
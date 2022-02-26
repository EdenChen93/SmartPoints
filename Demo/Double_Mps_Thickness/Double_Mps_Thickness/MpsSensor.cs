using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPSizectorS_DotNet;
using SmartPoints;

namespace Double_Mps_Thickness
{
    class MpsSensor : MPSizectorS_DotNet.MPSizectorS
    {

        public string DeviceSecName { get; set; }
        public MPSizectorS mps { get; set; }
        public MpsSensor ()
        {
            this.mps = new MPSizectorS(LogMediaType.Off);
            bool res= mps.UpdateDeviceList();
            if (res)
            {
                int n = mps.GetDeviceCount();
                string devicenamelist = "";
                for (int i = 0; i < mps.GetDeviceCount(); i++)
                {
                    devicenamelist += i+":"+mps.GetDeviceInfo(i).DeviceName + "\r\n";
                }
               bool IsOpen= mps.Open(mps.GetDeviceInfo(SmartPoints.SmartPoints.SmartPointsInputBox.ConvertToIntarray(SmartPoints.SmartPoints.SmartPointsInputBox.InputMessageBoxShow(devicenamelist))[0]));
                if (IsOpen)
                {
                    mps.WorkingMode = WorkingModeType.SuperPrecise3D;
                    mps.TriggerSource = TriggerSourceType.SoftTriggerOnly;
                    mps.DataOutMode = DataOutModeType.FloatPointCloud;
                    mps.HoldState = 0;
                    mps.SetDataCallBack(DataHandler);
                }
                else
                {
                    Console.WriteLine("OpenFiled");
                }
            } 
        }
        public void Tirgger()
        {

        }
        private void DataHandler(DataFormatType DataFormat, UnmanagedDataFrameUndefinedStruct DataFrame)
        {
            if (DataFormat == DataFormatType.FloatPointCloud)
            {
                DataProcessEvent(SmartPoints.SmartPoints.SP_FileReader.GetSpcPointsFromMSSensor(DataFrame));
            }
        }
        public delegate void DataProcess(SmartPoints.SmartPoints.SmartPointsCloud cloud);
        public event DataProcess DataProcessEvent;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPSizectorDotNet;

namespace SPCWTest
{
    class MegaPhaseHD
    {
        public MPSizectorDotNet.MPSizector HDSensor = new MPSizector();
        private MPSizectorSelectDialog HDSelectDialog;
        private MP3DFrameManaged[] buffer3DCallBack;
        private MP3DFrameManaged current3D;
        private bool OpenSensor()
        {
            HDSelectDialog = new MPSizectorSelectDialog();
            if (HDSelectDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                HDSensor = HDSelectDialog.Device;
                HDSensor.HoldState = false;
                HDSensor.WorkingMode = WorkingModeType.WorkingMode3DSuperPrecise;
                HDSensor.ExposureTime = 1000;
                HDSensor.PreProcessLoopNum = 5;
                HDSensor.TriggerSource = TriggerSourceType.TriggerSourceNoTrigger;
                buffer3DCallBack = HDSensor.Set3DDataCallBackManaged(ScanCallBack, 10);
                
                return true;
            }
            else
            {
                return false;
            }
        }
        private void ScanCallBack(byte BufferIndex)
        {
            current3D = buffer3DCallBack[BufferIndex];
            ProcessHDEvent(SmartPoints.SmartPoints.SP_FileReader.GetSpcPointsFromM3dSensor(current3D, HDSensor.CurrentDeviceInfo.DeviceName, "CurrentM3dm"));
        }
        public MegaPhaseHD ()
        {
            OpenSensor();
        }
        public delegate void ProcessHDdel(SmartPoints.SmartPoints.SmartPointsCloud cloud);
        public event ProcessHDdel ProcessHDEvent;
    }
}

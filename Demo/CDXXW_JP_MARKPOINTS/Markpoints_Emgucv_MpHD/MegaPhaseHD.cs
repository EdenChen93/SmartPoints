using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MPSizectorDotNet;


namespace Markpoints_Emgucv_MpHD
{
    class MegaPhaseHD
    {
        ThreadStart CtnthreadStart;
        Thread Ctnthread;
        public MPSizectorDotNet.MPSizector HDSensor = new MPSizector();
        public bool ISReadyToTrigger = false;
        private MPSizectorSelectDialog HDSelectDialog;
        private MP3DFrameManaged[] buffer3DCallBack;
        private MP3DFrameManaged current3D;
        private void ScanCallBack(byte BufferIndex)
        {
            current3D = buffer3DCallBack[BufferIndex];
            ProcessHDEvent(SmartPoints.SmartPoints.SP_FileReader.GetSpcPointsFromM3dSensor(current3D, HDSensor.CurrentDeviceInfo.DeviceName, "CurrentM3dm"));
        }
        private bool OpenSensor()
        {
            HDSelectDialog = new MPSizectorSelectDialog();
            if (HDSelectDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                HDSensor = HDSelectDialog.Device;
                HDSensor.HoldState = false;
                HDSensor.WorkingMode = WorkingModeType.WorkingMode3DSuperPrecise;
                HDSensor.ExposureTime = 2500;
                HDSensor.LEDCurrent = 255;
                HDSensor.PreProcessLoopNum = 5;
                HDSensor.TriggerSource = TriggerSourceType.TriggerSourceNoTrigger;
                buffer3DCallBack = HDSensor.Set3DDataCallBackManaged(ScanCallBack, 10);
                CtnthreadStart = new ThreadStart(CtnSoftTrigger);
                Ctnthread = new Thread(CtnthreadStart);
                Ctnthread.Name = "HD_CtnTrigger_Thread";
                Ctnthread.Start();
                return true;
            }
            else
            {
                return false;
            }
        }
        private void CtnSoftTrigger()
        {
            while (true)
            {
                while (ISReadyToTrigger && HDSensor.IsBusy)
                {
                    HDSensor.FireSoftwareTrigger();
                }

            }
        }
        public void StartCtn()
        {
            ISReadyToTrigger = true;
        }
        public void EndCtn()
        {
            ISReadyToTrigger = false;
        }


        public MegaPhaseHD ()
        {
            OpenSensor();
        }
        public delegate void ProcessHDdel(SmartPoints.SmartPoints.SmartPointsCloud cloud);
        public event ProcessHDdel ProcessHDEvent;
    }
}

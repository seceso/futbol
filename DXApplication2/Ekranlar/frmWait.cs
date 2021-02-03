using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraWaitForm;

namespace DXApplication2.Ekranlar
{
    public partial class frmWait : WaitForm
    {
        public frmWait()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            switch ((WaitFormCommand)cmd)
            {
                case WaitFormCommand.SetProgressBarVisible:
                    //Gelen komut eğer SetProgressBarVisible komutu ise, arg bir boolean değer olmalıdır. Bu değeri biz progressbarımızın Visible özelliğine aktarıyoruz.
                    this.progressBarControl1.Visible = (bool)arg;
                    break;
                case WaitFormCommand.SetProgressBarMaximum:

                    //Gelen komut eğer SetProgressBarMaximum komutu ise, arg bir integer değer olmalıdır. Bu değeri biz progressbarımızın Properties.Maximum özelliğine aktarıyoruz.
                    this.progressBarControl1.Properties.Maximum = (int)arg;
                    break;
                case WaitFormCommand.SetProgressBarPosition:
                    //Gelen komut eğer SetProgressBarPosition komutu ise, arg bir integer değer olmalıdır. Bu değeri biz progressbarımızın Position özelliğine aktarıyoruz.
                    this.progressBarControl1.Position = (int)arg;
                    break;
            }

            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {
            SetProgressBarVisible,
            SetProgressBarMaximum,
            SetProgressBarPosition
        }
    }
}
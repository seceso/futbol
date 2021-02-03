namespace DXApplication2.Ekranlar
{
    partial class FiksturUC
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colMacNo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTurnuvaAdi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEvSahibi = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEvSahibiSkor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMisafir = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMisafirSkor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTarih = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFikstur = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(576, 392);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colMacNo,
            this.colTurnuvaAdi,
            this.colEvSahibi,
            this.colEvSahibiSkor,
            this.colMisafir,
            this.colMisafirSkor,
            this.colTarih,
            this.colFikstur});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.GroupCount = 1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsBehavior.AutoExpandAllGroups = true;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;
            this.gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colTurnuvaAdi, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colTarih, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colMacNo
            // 
            this.colMacNo.Caption = "Maç No";
            this.colMacNo.FieldName = "Id";
            this.colMacNo.Name = "colMacNo";
            this.colMacNo.OptionsColumn.AllowEdit = false;
            this.colMacNo.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "Id", "{0}")});
            this.colMacNo.Visible = true;
            this.colMacNo.VisibleIndex = 0;
            // 
            // colTurnuvaAdi
            // 
            this.colTurnuvaAdi.Caption = "Turnuva Adı";
            this.colTurnuvaAdi.FieldName = "TournamentName";
            this.colTurnuvaAdi.GroupInterval = DevExpress.XtraGrid.ColumnGroupInterval.DisplayText;
            this.colTurnuvaAdi.Name = "colTurnuvaAdi";
            this.colTurnuvaAdi.Visible = true;
            this.colTurnuvaAdi.VisibleIndex = 7;
            // 
            // colEvSahibi
            // 
            this.colEvSahibi.Caption = "Ev";
            this.colEvSahibi.FieldName = "HomeTeamName";
            this.colEvSahibi.Name = "colEvSahibi";
            this.colEvSahibi.OptionsColumn.AllowEdit = false;
            this.colEvSahibi.Visible = true;
            this.colEvSahibi.VisibleIndex = 1;
            // 
            // colEvSahibiSkor
            // 
            this.colEvSahibiSkor.Caption = "Skor";
            this.colEvSahibiSkor.FieldName = "HomeTeamScore";
            this.colEvSahibiSkor.Name = "colEvSahibiSkor";
            this.colEvSahibiSkor.OptionsColumn.AllowEdit = false;
            this.colEvSahibiSkor.Visible = true;
            this.colEvSahibiSkor.VisibleIndex = 2;
            // 
            // colMisafir
            // 
            this.colMisafir.Caption = "Misafir";
            this.colMisafir.FieldName = "AwayTeamName";
            this.colMisafir.Name = "colMisafir";
            this.colMisafir.OptionsColumn.AllowEdit = false;
            this.colMisafir.Visible = true;
            this.colMisafir.VisibleIndex = 3;
            // 
            // colMisafirSkor
            // 
            this.colMisafirSkor.Caption = "Skor";
            this.colMisafirSkor.FieldName = "AwayTeamScore";
            this.colMisafirSkor.Name = "colMisafirSkor";
            this.colMisafirSkor.OptionsColumn.AllowEdit = false;
            this.colMisafirSkor.Visible = true;
            this.colMisafirSkor.VisibleIndex = 4;
            // 
            // colTarih
            // 
            this.colTarih.Caption = "Tarih";
            this.colTarih.FieldName = "StartDate";
            this.colTarih.Name = "colTarih";
            this.colTarih.OptionsColumn.AllowEdit = false;
            this.colTarih.Visible = true;
            this.colTarih.VisibleIndex = 5;
            // 
            // colFikstur
            // 
            this.colFikstur.Caption = "Fikstür";
            this.colFikstur.FieldName = "MatchDay";
            this.colFikstur.Name = "colFikstur";
            this.colFikstur.OptionsColumn.AllowEdit = false;
            this.colFikstur.Visible = true;
            this.colFikstur.VisibleIndex = 6;
            // 
            // FiksturUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl1);
            this.Name = "FiksturUC";
            this.Size = new System.Drawing.Size(576, 392);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        public DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Columns.GridColumn colMacNo;
        private DevExpress.XtraGrid.Columns.GridColumn colEvSahibi;
        private DevExpress.XtraGrid.Columns.GridColumn colEvSahibiSkor;
        private DevExpress.XtraGrid.Columns.GridColumn colMisafir;
        private DevExpress.XtraGrid.Columns.GridColumn colMisafirSkor;
        private DevExpress.XtraGrid.Columns.GridColumn colTarih;
        private DevExpress.XtraGrid.Columns.GridColumn colFikstur;
        private DevExpress.XtraGrid.Columns.GridColumn colTurnuvaAdi;
    }
}

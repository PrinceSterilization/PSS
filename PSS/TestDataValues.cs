//TestDataValues.cs
// AUTHOR       : ALEXANDER M. DELA CRUZ
// TITLE        : Software Developer
// DATE         : 10-19-2015
// LOCATION     : GIBRALTAR LABORATORIES, INC.
// DESCRIPTION  : Test Results Data File Maintenance

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace PSS
{
    public partial class TestDataValues : Form
    {
        public string pubCmpy;
        public Int64 nLogNo;
        public int nServiceCode;
        public int nSponsorID;
        public int nMode = 1;
        public byte nEdit = 0;
        public Int32 nRptNo;
        public string strFormat;

        private static string[,] arrLabels = new string[6, 15];
        private DataTable dtMain = new DataTable();
        private DataTable dtSampleSC = new DataTable();

        private byte nDF = 1;

        public TestDataValues()
        {
            InitializeComponent();
            LoadAnalysts();
        }

        private void TestDataValues_Load(object sender, EventArgs e)
        {
            txtGBLNo.Text = nLogNo.ToString();
            txtSC.Text = nServiceCode.ToString();
            txtSpID.Text = nSponsorID.ToString();

            LoadDataFormats();
            if (nDF == 0)
            {
                btnCancel_Click(null, null);
                return;
            }

            SetUpDGV();
            cboDataFormats_SelectedIndexChanged(null, null);

            dtMain.Columns.Add("ExperimentNo", typeof(Int32));
            dtMain.Columns.Add("PageNo", typeof(Int32));
            dtMain.Columns.Add("AnalystID", typeof(Int16));
            dtMain.Columns.Add("Media", typeof(string));
            dtMain.Columns.Add("TableFormat", typeof(Int16));
            bsMain.DataSource = dtMain;

            txtExptNo.DataBindings.Add("Text", bsMain, "ExperimentNo", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtPageNo.DataBindings.Add("Text", bsMain, "PageNo", true, DataSourceUpdateMode.OnPropertyChanged, "");
            cboAnalysts.DataBindings.Add("SelectedValue", bsMain, "AnalystID", true, DataSourceUpdateMode.OnPropertyChanged, "");
            cboTableFormats.DataBindings.Add("SelectedValue", bsMain, "TableFormat", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtAnalystID.DataBindings.Add("Text", bsMain, "AnalystID", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtMedia.DataBindings.Add("Text", bsMain, "Media", true, DataSourceUpdateMode.OnPropertyChanged, "");

            //Table Setup for Test Data Values
            dtSampleSC.Columns.Add("LogNo", typeof(string));
            dtSampleSC.Columns.Add("SlashNo", typeof(string));
            dtSampleSC.Columns.Add("V1", typeof(string));
            dtSampleSC.Columns.Add("V2", typeof(string));
            dtSampleSC.Columns.Add("V3", typeof(string));
            dtSampleSC.Columns.Add("V4", typeof(string));
            dtSampleSC.Columns.Add("V5", typeof(string));
            dtSampleSC.Columns.Add("V6", typeof(string));
            dtSampleSC.Columns.Add("V7", typeof(string));
            dtSampleSC.Columns.Add("V8", typeof(string));
            dtSampleSC.Columns.Add("V9", typeof(string));
            dtSampleSC.Columns.Add("V10", typeof(string));
            dtSampleSC.Columns.Add("V11", typeof(string));
            dtSampleSC.Columns.Add("V12", typeof(string));
            dtSampleSC.Columns.Add("V13", typeof(string));
            dtSampleSC.Columns.Add("V14", typeof(string));
            dtSampleSC.Columns.Add("V15", typeof(string));
            dtSampleSC.Columns.Add("V16", typeof(string));
            dtSampleSC.Columns.Add("V17", typeof(string));
            dtSampleSC.Columns.Add("V18", typeof(string));
            dtSampleSC.Columns.Add("V19", typeof(string));
            dtSampleSC.Columns.Add("V20", typeof(string));
            dtSampleSC.Columns.Add("V21", typeof(string));
            dtSampleSC.Columns.Add("V22", typeof(string));
            dtSampleSC.Columns.Add("V23", typeof(string));
            dtSampleSC.Columns.Add("V24", typeof(string));
            dtSampleSC.Columns.Add("V25", typeof(string));
            dtSampleSC.Columns.Add("V26", typeof(string));
            dtSampleSC.Columns.Add("V27", typeof(string));
            dtSampleSC.Columns.Add("V28", typeof(string));
            dtSampleSC.Columns.Add("V29", typeof(string));
            dtSampleSC.Columns.Add("V30", typeof(string));
            dtSampleSC.Columns.Add("V31", typeof(string));
            dtSampleSC.Columns.Add("V32", typeof(string));
            dtSampleSC.Columns.Add("V33", typeof(string));
            dtSampleSC.Columns.Add("V34", typeof(string));
            dtSampleSC.Columns.Add("V35", typeof(string));
            dtSampleSC.Columns.Add("V36", typeof(string));
            dtSampleSC.Columns.Add("V37", typeof(string));
            dtSampleSC.Columns.Add("V38", typeof(string));
            dtSampleSC.Columns.Add("V39", typeof(string));
            dtSampleSC.Columns.Add("V40", typeof(string));
            dtSampleSC.Columns.Add("V41", typeof(string));
            dtSampleSC.Columns.Add("V42", typeof(string));
            dtSampleSC.Columns.Add("V43", typeof(string));
            dtSampleSC.Columns.Add("V44", typeof(string));
            dtSampleSC.Columns.Add("V45", typeof(string));
            dtSampleSC.Columns.Add("V46", typeof(string));
            dtSampleSC.Columns.Add("V47", typeof(string));
            dtSampleSC.Columns.Add("V48", typeof(string));
            dtSampleSC.Columns.Add("V49", typeof(string));
            dtSampleSC.Columns.Add("V50", typeof(string));
            dtSampleSC.Columns.Add("V51", typeof(string));
            dtSampleSC.Columns.Add("V52", typeof(string));
            dtSampleSC.Columns.Add("V53", typeof(string));
            dtSampleSC.Columns.Add("V54", typeof(string));
            dtSampleSC.Columns.Add("V55", typeof(string));
            dtSampleSC.Columns.Add("V56", typeof(string));
            dtSampleSC.Columns.Add("V57", typeof(string));
            dtSampleSC.Columns.Add("V58", typeof(string));
            dtSampleSC.Columns.Add("V59", typeof(string));
            dtSampleSC.Columns.Add("V60", typeof(string));
            dtSampleSC.Columns.Add("V61", typeof(string));
            dtSampleSC.Columns.Add("V62", typeof(string));
            dtSampleSC.Columns.Add("V63", typeof(string));
            dtSampleSC.Columns.Add("V64", typeof(string));
            dtSampleSC.Columns.Add("V65", typeof(string));
            dtSampleSC.Columns.Add("V66", typeof(string));
            dtSampleSC.Columns.Add("V67", typeof(string));
            dtSampleSC.Columns.Add("V68", typeof(string));
            dtSampleSC.Columns.Add("V69", typeof(string));
            dtSampleSC.Columns.Add("V70", typeof(string));
            dtSampleSC.Columns.Add("V71", typeof(string));
            dtSampleSC.Columns.Add("V72", typeof(string));
            dtSampleSC.Columns.Add("V73", typeof(string));
            dtSampleSC.Columns.Add("V74", typeof(string));
            dtSampleSC.Columns.Add("V75", typeof(string));
            dtSampleSC.Columns.Add("V76", typeof(string));
            dtSampleSC.Columns.Add("V77", typeof(string));
            dtSampleSC.Columns.Add("V78", typeof(string));
            dtSampleSC.Columns.Add("V79", typeof(string));
            dtSampleSC.Columns.Add("V80", typeof(string));
            dtSampleSC.Columns.Add("V81", typeof(string));
            dtSampleSC.Columns.Add("V82", typeof(string));
            dtSampleSC.Columns.Add("V83", typeof(string));
            dtSampleSC.Columns.Add("V84", typeof(string));
            dtSampleSC.Columns.Add("V85", typeof(string));
            dtSampleSC.Columns.Add("V86", typeof(string));
            dtSampleSC.Columns.Add("V87", typeof(string));
            dtSampleSC.Columns.Add("V88", typeof(string));
            dtSampleSC.Columns.Add("V89", typeof(string));
            dtSampleSC.Columns.Add("V90", typeof(string));
            dtSampleSC.Columns.Add("V91", typeof(string));
            dtSampleSC.Columns.Add("V92", typeof(string));
            dtSampleSC.Columns.Add("V93", typeof(string));
            dtSampleSC.Columns.Add("V94", typeof(string));
            dtSampleSC.Columns.Add("V95", typeof(string));
            dtSampleSC.Columns.Add("V96", typeof(string));
            dtSampleSC.Columns.Add("V97", typeof(string));
            dtSampleSC.Columns.Add("V98", typeof(string));
            dtSampleSC.Columns.Add("V99", typeof(string));
            dtSampleSC.Columns.Add("V100", typeof(string));
            dtSampleSC.Columns.Add("V101", typeof(string));
            dtSampleSC.Columns.Add("V102", typeof(string));
            dtSampleSC.Columns.Add("V103", typeof(string));
            dtSampleSC.Columns.Add("V104", typeof(string));
            dtSampleSC.Columns.Add("V105", typeof(string));
            dtSampleSC.Columns.Add("V106", typeof(string));
            dtSampleSC.Columns.Add("V107", typeof(string));
            dtSampleSC.Columns.Add("V108", typeof(string));
            dtSampleSC.Columns.Add("V109", typeof(string));
            dtSampleSC.Columns.Add("V110", typeof(string));
            dtSampleSC.Columns.Add("V111", typeof(string));
            dtSampleSC.Columns.Add("V112", typeof(string));
            dtSampleSC.Columns.Add("V113", typeof(string));
            dtSampleSC.Columns.Add("V114", typeof(string));
            dtSampleSC.Columns.Add("V115", typeof(string));
            dtSampleSC.Columns.Add("V116", typeof(string));
            dtSampleSC.Columns.Add("V117", typeof(string));
            dtSampleSC.Columns.Add("V118", typeof(string));
            dtSampleSC.Columns.Add("V119", typeof(string));
            dtSampleSC.Columns.Add("V120", typeof(string));
            dtSampleSC.Columns.Add("V121", typeof(string));
            dtSampleSC.Columns.Add("V122", typeof(string));
            dtSampleSC.Columns.Add("V123", typeof(string));
            dtSampleSC.Columns.Add("V124", typeof(string));
            dtSampleSC.Columns.Add("V125", typeof(string));
            dtSampleSC.Columns.Add("V126", typeof(string));
            dtSampleSC.Columns.Add("V127", typeof(string));
            dtSampleSC.Columns.Add("V128", typeof(string));
            dtSampleSC.Columns.Add("V129", typeof(string));
            dtSampleSC.Columns.Add("V130", typeof(string));
            dtSampleSC.Columns.Add("V131", typeof(string));
            dtSampleSC.Columns.Add("V132", typeof(string));
            dtSampleSC.Columns.Add("V133", typeof(string));
            dtSampleSC.Columns.Add("V134", typeof(string));
            dtSampleSC.Columns.Add("V135", typeof(string));
            dtSampleSC.Columns.Add("V136", typeof(string));
            dtSampleSC.Columns.Add("V137", typeof(string));
            dtSampleSC.Columns.Add("V138", typeof(string));
            dtSampleSC.Columns.Add("V139", typeof(string));
            dtSampleSC.Columns.Add("V140", typeof(string));
            dtSampleSC.Columns.Add("V141", typeof(string));
            dtSampleSC.Columns.Add("V142", typeof(string));
            dtSampleSC.Columns.Add("V143", typeof(string));
            dtSampleSC.Columns.Add("V144", typeof(string));
            dtSampleSC.Columns.Add("V145", typeof(string));
            dtSampleSC.Columns.Add("V146", typeof(string));
            dtSampleSC.Columns.Add("V147", typeof(string));
            dtSampleSC.Columns.Add("V148", typeof(string));
            dtSampleSC.Columns.Add("V149", typeof(string));
            dtSampleSC.Columns.Add("V150", typeof(string));
            dtSampleSC.Columns.Add("V151", typeof(string));
            dtSampleSC.Columns.Add("V152", typeof(string));
            dtSampleSC.Columns.Add("V153", typeof(string));
            dtSampleSC.Columns.Add("V154", typeof(string));
            dtSampleSC.Columns.Add("V155", typeof(string));
            dtSampleSC.Columns.Add("V156", typeof(string));
            dtSampleSC.Columns.Add("V157", typeof(string));
            dtSampleSC.Columns.Add("V158", typeof(string));
            dtSampleSC.Columns.Add("V159", typeof(string));
            dtSampleSC.Columns.Add("V160", typeof(string));
            dtSampleSC.Columns.Add("V161", typeof(string));
            dtSampleSC.Columns.Add("V162", typeof(string));
            dtSampleSC.Columns.Add("V163", typeof(string));
            dtSampleSC.Columns.Add("V164", typeof(string));
            dtSampleSC.Columns.Add("V165", typeof(string));
            dtSampleSC.Columns.Add("V166", typeof(string));
            dtSampleSC.Columns.Add("V167", typeof(string));
            dtSampleSC.Columns.Add("V168", typeof(string));
            dtSampleSC.Columns.Add("V169", typeof(string));
            dtSampleSC.Columns.Add("V170", typeof(string));
            dtSampleSC.Columns.Add("V171", typeof(string));
            dtSampleSC.Columns.Add("V172", typeof(string));
            dtSampleSC.Columns.Add("V173", typeof(string));
            dtSampleSC.Columns.Add("V174", typeof(string));
            dtSampleSC.Columns.Add("V175", typeof(string));
            dtSampleSC.Columns.Add("V176", typeof(string));
            dtSampleSC.Columns.Add("V177", typeof(string));
            dtSampleSC.Columns.Add("V178", typeof(string));
            dtSampleSC.Columns.Add("V179", typeof(string));
            dtSampleSC.Columns.Add("V180", typeof(string));
            dtSampleSC.Columns.Add("SD1", typeof(bool));
            dtSampleSC.Columns.Add("SD2", typeof(bool));
            dtSampleSC.Columns.Add("SD3", typeof(bool));
            dtSampleSC.Columns.Add("SD4", typeof(bool));
            dtSampleSC.Columns.Add("Notes", typeof(string));
            bsData.DataSource = dtSampleSC;
            //Databindings for Test Data Values
            txtSlashNo.DataBindings.Add("Text", bsData, "SlashNo");
            txtDataValue1.DataBindings.Add("Text", bsData, "V1", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue2.DataBindings.Add("Text", bsData, "V2", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue3.DataBindings.Add("Text", bsData, "V3", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue4.DataBindings.Add("Text", bsData, "V4", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue5.DataBindings.Add("Text", bsData, "V5", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue6.DataBindings.Add("Text", bsData, "V6", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue7.DataBindings.Add("Text", bsData, "V7", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue8.DataBindings.Add("Text", bsData, "V8", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue9.DataBindings.Add("Text", bsData, "V9", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue10.DataBindings.Add("Text", bsData, "V10", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue11.DataBindings.Add("Text", bsData, "V11", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue12.DataBindings.Add("Text", bsData, "V12", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue13.DataBindings.Add("Text", bsData, "V13", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue14.DataBindings.Add("Text", bsData, "V14", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue15.DataBindings.Add("Text", bsData, "V15", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue16.DataBindings.Add("Text", bsData, "V16", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue17.DataBindings.Add("Text", bsData, "V17", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue18.DataBindings.Add("Text", bsData, "V18", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue19.DataBindings.Add("Text", bsData, "V19", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue20.DataBindings.Add("Text", bsData, "V20", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue21.DataBindings.Add("Text", bsData, "V21", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue22.DataBindings.Add("Text", bsData, "V22", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue23.DataBindings.Add("Text", bsData, "V23", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue24.DataBindings.Add("Text", bsData, "V24", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue25.DataBindings.Add("Text", bsData, "V25", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue26.DataBindings.Add("Text", bsData, "V26", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue27.DataBindings.Add("Text", bsData, "V27", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue28.DataBindings.Add("Text", bsData, "V28", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue29.DataBindings.Add("Text", bsData, "V29", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue30.DataBindings.Add("Text", bsData, "V30", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue31.DataBindings.Add("Text", bsData, "V31", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue32.DataBindings.Add("Text", bsData, "V32", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue33.DataBindings.Add("Text", bsData, "V33", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue34.DataBindings.Add("Text", bsData, "V34", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue35.DataBindings.Add("Text", bsData, "V35", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue36.DataBindings.Add("Text", bsData, "V36", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue37.DataBindings.Add("Text", bsData, "V37", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue38.DataBindings.Add("Text", bsData, "V38", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue39.DataBindings.Add("Text", bsData, "V39", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue40.DataBindings.Add("Text", bsData, "V40", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue41.DataBindings.Add("Text", bsData, "V41", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue42.DataBindings.Add("Text", bsData, "V42", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue43.DataBindings.Add("Text", bsData, "V43", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue44.DataBindings.Add("Text", bsData, "V44", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue45.DataBindings.Add("Text", bsData, "V45", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue46.DataBindings.Add("Text", bsData, "V46", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue47.DataBindings.Add("Text", bsData, "V47", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue48.DataBindings.Add("Text", bsData, "V48", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue49.DataBindings.Add("Text", bsData, "V49", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue50.DataBindings.Add("Text", bsData, "V50", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue51.DataBindings.Add("Text", bsData, "V51", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue52.DataBindings.Add("Text", bsData, "V52", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue53.DataBindings.Add("Text", bsData, "V53", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue54.DataBindings.Add("Text", bsData, "V54", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue55.DataBindings.Add("Text", bsData, "V55", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue56.DataBindings.Add("Text", bsData, "V56", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue57.DataBindings.Add("Text", bsData, "V57", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue58.DataBindings.Add("Text", bsData, "V58", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue59.DataBindings.Add("Text", bsData, "V59", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue60.DataBindings.Add("Text", bsData, "V60", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue61.DataBindings.Add("Text", bsData, "V61", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue62.DataBindings.Add("Text", bsData, "V62", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue63.DataBindings.Add("Text", bsData, "V63", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue64.DataBindings.Add("Text", bsData, "V64", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue65.DataBindings.Add("Text", bsData, "V65", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue66.DataBindings.Add("Text", bsData, "V66", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue67.DataBindings.Add("Text", bsData, "V67", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue68.DataBindings.Add("Text", bsData, "V68", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue69.DataBindings.Add("Text", bsData, "V69", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue70.DataBindings.Add("Text", bsData, "V70", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue71.DataBindings.Add("Text", bsData, "V71", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue72.DataBindings.Add("Text", bsData, "V72", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue73.DataBindings.Add("Text", bsData, "V73", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue74.DataBindings.Add("Text", bsData, "V74", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue75.DataBindings.Add("Text", bsData, "V75", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue76.DataBindings.Add("Text", bsData, "V76", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue77.DataBindings.Add("Text", bsData, "V77", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue78.DataBindings.Add("Text", bsData, "V78", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue79.DataBindings.Add("Text", bsData, "V79", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue80.DataBindings.Add("Text", bsData, "V80", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue81.DataBindings.Add("Text", bsData, "V81", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue82.DataBindings.Add("Text", bsData, "V82", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue83.DataBindings.Add("Text", bsData, "V83", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue84.DataBindings.Add("Text", bsData, "V84", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue85.DataBindings.Add("Text", bsData, "V85", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue86.DataBindings.Add("Text", bsData, "V86", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue87.DataBindings.Add("Text", bsData, "V87", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue88.DataBindings.Add("Text", bsData, "V88", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue89.DataBindings.Add("Text", bsData, "V89", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue90.DataBindings.Add("Text", bsData, "V90", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue91.DataBindings.Add("Text", bsData, "V91", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue92.DataBindings.Add("Text", bsData, "V92", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue93.DataBindings.Add("Text", bsData, "V93", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue94.DataBindings.Add("Text", bsData, "V94", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue95.DataBindings.Add("Text", bsData, "V95", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue96.DataBindings.Add("Text", bsData, "V96", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue97.DataBindings.Add("Text", bsData, "V97", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue98.DataBindings.Add("Text", bsData, "V98", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue99.DataBindings.Add("Text", bsData, "V99", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue100.DataBindings.Add("Text", bsData, "V100", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue101.DataBindings.Add("Text", bsData, "V101", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue102.DataBindings.Add("Text", bsData, "V102", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue103.DataBindings.Add("Text", bsData, "V103", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue104.DataBindings.Add("Text", bsData, "V104", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue105.DataBindings.Add("Text", bsData, "V105", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue106.DataBindings.Add("Text", bsData, "V106", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue107.DataBindings.Add("Text", bsData, "V107", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue108.DataBindings.Add("Text", bsData, "V108", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue109.DataBindings.Add("Text", bsData, "V109", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue110.DataBindings.Add("Text", bsData, "V110", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue111.DataBindings.Add("Text", bsData, "V111", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue112.DataBindings.Add("Text", bsData, "V112", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue113.DataBindings.Add("Text", bsData, "V113", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue114.DataBindings.Add("Text", bsData, "V114", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue115.DataBindings.Add("Text", bsData, "V115", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue116.DataBindings.Add("Text", bsData, "V116", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue117.DataBindings.Add("Text", bsData, "V117", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue118.DataBindings.Add("Text", bsData, "V118", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue119.DataBindings.Add("Text", bsData, "V119", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue120.DataBindings.Add("Text", bsData, "V120", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue121.DataBindings.Add("Text", bsData, "V121", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue122.DataBindings.Add("Text", bsData, "V122", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue123.DataBindings.Add("Text", bsData, "V123", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue124.DataBindings.Add("Text", bsData, "V124", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue125.DataBindings.Add("Text", bsData, "V125", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue126.DataBindings.Add("Text", bsData, "V126", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue127.DataBindings.Add("Text", bsData, "V127", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue128.DataBindings.Add("Text", bsData, "V128", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue129.DataBindings.Add("Text", bsData, "V129", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue130.DataBindings.Add("Text", bsData, "V130", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue131.DataBindings.Add("Text", bsData, "V131", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue132.DataBindings.Add("Text", bsData, "V132", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue133.DataBindings.Add("Text", bsData, "V133", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue134.DataBindings.Add("Text", bsData, "V134", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue135.DataBindings.Add("Text", bsData, "V135", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue136.DataBindings.Add("Text", bsData, "V136", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue137.DataBindings.Add("Text", bsData, "V137", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue138.DataBindings.Add("Text", bsData, "V138", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue139.DataBindings.Add("Text", bsData, "V139", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue140.DataBindings.Add("Text", bsData, "V140", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue141.DataBindings.Add("Text", bsData, "V141", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue142.DataBindings.Add("Text", bsData, "V142", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue143.DataBindings.Add("Text", bsData, "V143", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue144.DataBindings.Add("Text", bsData, "V144", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue145.DataBindings.Add("Text", bsData, "V145", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue146.DataBindings.Add("Text", bsData, "V146", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue147.DataBindings.Add("Text", bsData, "V147", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue148.DataBindings.Add("Text", bsData, "V148", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue149.DataBindings.Add("Text", bsData, "V149", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue150.DataBindings.Add("Text", bsData, "V150", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue151.DataBindings.Add("Text", bsData, "V151", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue152.DataBindings.Add("Text", bsData, "V152", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue153.DataBindings.Add("Text", bsData, "V153", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue154.DataBindings.Add("Text", bsData, "V154", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue155.DataBindings.Add("Text", bsData, "V155", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue156.DataBindings.Add("Text", bsData, "V156", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue157.DataBindings.Add("Text", bsData, "V157", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue158.DataBindings.Add("Text", bsData, "V158", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue159.DataBindings.Add("Text", bsData, "V159", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue160.DataBindings.Add("Text", bsData, "V160", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue161.DataBindings.Add("Text", bsData, "V161", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue162.DataBindings.Add("Text", bsData, "V162", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue163.DataBindings.Add("Text", bsData, "V163", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue164.DataBindings.Add("Text", bsData, "V164", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue165.DataBindings.Add("Text", bsData, "V165", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue166.DataBindings.Add("Text", bsData, "V166", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue167.DataBindings.Add("Text", bsData, "V167", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue168.DataBindings.Add("Text", bsData, "V168", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue169.DataBindings.Add("Text", bsData, "V169", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue170.DataBindings.Add("Text", bsData, "V170", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue171.DataBindings.Add("Text", bsData, "V171", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue172.DataBindings.Add("Text", bsData, "V172", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue173.DataBindings.Add("Text", bsData, "V173", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue174.DataBindings.Add("Text", bsData, "V174", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue175.DataBindings.Add("Text", bsData, "V175", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue176.DataBindings.Add("Text", bsData, "V176", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue177.DataBindings.Add("Text", bsData, "V177", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue178.DataBindings.Add("Text", bsData, "V178", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue179.DataBindings.Add("Text", bsData, "V179", true, DataSourceUpdateMode.OnPropertyChanged, "");
            txtDataValue180.DataBindings.Add("Text", bsData, "V180", true, DataSourceUpdateMode.OnPropertyChanged, "");
            chkSD1.DataBindings.Add("Checked", bsData, "SD1", true, DataSourceUpdateMode.OnPropertyChanged, false);
            chkSD2.DataBindings.Add("Checked", bsData, "SD2", true, DataSourceUpdateMode.OnPropertyChanged, false);
            chkSD3.DataBindings.Add("Checked", bsData, "SD3", true, DataSourceUpdateMode.OnPropertyChanged, false);
            chkSD4.DataBindings.Add("Checked", bsData, "SD4", true, DataSourceUpdateMode.OnPropertyChanged, false);
            txtNote.DataBindings.Add("Text", bsData, "Notes", true, DataSourceUpdateMode.OnPropertyChanged, "");
            LoadDataValues();
            this.Top = 155; this.Left = 5;
            bnData.Enabled = true;
        }

        private void OpenControls(Control c, bool b)
        {
            foreach (Control Ctrl in c.Controls)
            {
                switch (Ctrl.GetType().ToString())
                {
                    case "System.Windows.Forms.CheckBox":
                        ((CheckBox)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.RadioButton":
                        ((RadioButton)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.TextBox":
                        ((TextBox)Ctrl).ReadOnly = !b;
                        break;

                    case "GISControls.TextBoxChar": //User Control
                        ((GISControls.TextBoxChar)Ctrl).ReadOnly = !b;
                        break;

                    case "GISControls.TextBoxAdjHt": //User Control
                        ((GISControls.TextBoxAdjHt)Ctrl).ReadOnly = !b;
                        break;

                    case "System.Windows.Forms.RichTextBox":
                        ((RichTextBox)Ctrl).ReadOnly = !b;
                        break;

                    case "System.Windows.Forms.ComboBox":
                        ((ComboBox)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.MaskedTextBox":
                        ((MaskedTextBox)Ctrl).ReadOnly = !b;
                        break;

                    case "System.Windows.Forms.DateTimePicker":
                        ((DateTimePicker)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.DataGridView":
                        ((DataGridView)Ctrl).Enabled = b;
                        break;

                    case "System.Windows.Forms.GroupBox":
                        ((GroupBox)Ctrl).Enabled = b;
                        break;

                    default:
                        if (Ctrl.Controls.Count > 0)
                            OpenControls(Ctrl, true);
                        break;
                }
            }
            c.Enabled = true;
        }

        private void LoadAnalysts()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.Employees.Analysts();
            if (dt == null)
            {
                return;
            }
            cboAnalysts.DataSource = dt;
            cboAnalysts.DisplayMember = "EmployeeName";
            cboAnalysts.ValueMember = "EmployeeID";

            DataRow dR = dt.NewRow();
            dR["EmployeeName"] = "--select--";
            dR["EmployeeID"] = "0";
            dt.Rows.InsertAt(dR, 0);
            cboAnalysts.SelectedIndex = 0;
        }

        private void LoadDataFormats()
        {
            DataTable dt = new DataTable();
            dt = PSSClass.Samples.SCDataFormats(nServiceCode, nSponsorID);
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No defined data form for this Service Code/Sponsor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                nDF = 0;
                this.Dispose();
                return;
            }
            cboTableFormats.DataSource = dt;
            cboTableFormats.DisplayMember = "FormatNo";
            cboTableFormats.ValueMember = "FormatNo";
            cboTableFormats.SelectedIndex = 0;
            txtTableRptID.Text = dt.Rows[0]["TableReportID"].ToString();
        }

        private void LoadDataValues()
        {
            dtSampleSC = PSSClass.Samples.ExTestDataValues(pubCmpy, nLogNo, nServiceCode);
            if (dtSampleSC == null)
            {
                return;
            }
            bsData.DataSource = dtSampleSC;
            bnData.BindingSource = bsData;
            try
            {
                DataRow dR = dtMain.NewRow();
                dR["ExperimentNo"] = dtSampleSC.Rows[0]["ExperimentNo"];
                dR["PageNo"] = dtSampleSC.Rows[0]["PageNo"];
                dR["AnalystID"] = dtSampleSC.Rows[0]["AnalystID"];
                dR["Media"] = dtSampleSC.Rows[0]["Media"];
                dR["TableFormat"] = dtSampleSC.Rows[0]["TableFormat"];
                dtMain.Rows.Add(dR);
                dtMain.AcceptChanges();
                bsMain.DataSource = dtMain;
                OpenControls(this, true); OpenControls(tabData, true);
            }
            catch { }
        }

        private void cboDataFormats_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboTableFormats.ValueMember != null)
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt = PSSClass.Samples.ExTestDataLabels(nServiceCode, nSponsorID, Convert.ToInt16(cboTableFormats.Text));
                    if (dt == null)
                    {
                        MessageBox.Show("Unexpected error. Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    lblDataLabel1.Text = dt.Rows[0]["L1"].ToString();
                    lblDataLabel2.Text = dt.Rows[0]["L2"].ToString();
                    lblDataLabel3.Text = dt.Rows[0]["L3"].ToString();
                    lblDataLabel4.Text = dt.Rows[0]["L4"].ToString();
                    lblDataLabel5.Text = dt.Rows[0]["L5"].ToString();
                    lblDataLabel6.Text = dt.Rows[0]["L6"].ToString();
                    lblDataLabel7.Text = dt.Rows[0]["L7"].ToString();
                    lblDataLabel8.Text = dt.Rows[0]["L8"].ToString();
                    lblDataLabel9.Text = dt.Rows[0]["L9"].ToString();
                    lblDataLabel10.Text = dt.Rows[0]["L10"].ToString();
                    lblDataLabel11.Text = dt.Rows[0]["L11"].ToString();
                    lblDataLabel12.Text = dt.Rows[0]["L12"].ToString();
                    lblDataLabel13.Text = dt.Rows[0]["L13"].ToString();
                    lblDataLabel14.Text = dt.Rows[0]["L14"].ToString();
                    lblDataLabel15.Text = dt.Rows[0]["L15"].ToString();
                    lblDataLabel16.Text = dt.Rows[0]["L16"].ToString();
                    lblDataLabel17.Text = dt.Rows[0]["L17"].ToString();
                    lblDataLabel18.Text = dt.Rows[0]["L18"].ToString();
                    lblDataLabel19.Text = dt.Rows[0]["L19"].ToString();
                    lblDataLabel20.Text = dt.Rows[0]["L20"].ToString();
                    lblDataLabel21.Text = dt.Rows[0]["L21"].ToString();
                    lblDataLabel22.Text = dt.Rows[0]["L22"].ToString();
                    lblDataLabel23.Text = dt.Rows[0]["L23"].ToString();
                    lblDataLabel24.Text = dt.Rows[0]["L24"].ToString();
                    lblDataLabel25.Text = dt.Rows[0]["L25"].ToString();
                    lblDataLabel26.Text = dt.Rows[0]["L26"].ToString();
                    lblDataLabel27.Text = dt.Rows[0]["L27"].ToString();
                    lblDataLabel28.Text = dt.Rows[0]["L28"].ToString();
                    lblDataLabel29.Text = dt.Rows[0]["L29"].ToString();
                    lblDataLabel30.Text = dt.Rows[0]["L30"].ToString();
                    lblDataLabel31.Text = dt.Rows[0]["L31"].ToString();
                    lblDataLabel32.Text = dt.Rows[0]["L32"].ToString();
                    lblDataLabel33.Text = dt.Rows[0]["L33"].ToString();
                    lblDataLabel34.Text = dt.Rows[0]["L34"].ToString();
                    lblDataLabel35.Text = dt.Rows[0]["L35"].ToString();
                    lblDataLabel36.Text = dt.Rows[0]["L36"].ToString();
                    lblDataLabel37.Text = dt.Rows[0]["L37"].ToString();
                    lblDataLabel38.Text = dt.Rows[0]["L38"].ToString();
                    lblDataLabel39.Text = dt.Rows[0]["L39"].ToString();
                    lblDataLabel40.Text = dt.Rows[0]["L40"].ToString();
                    lblDataLabel41.Text = dt.Rows[0]["L41"].ToString();
                    lblDataLabel42.Text = dt.Rows[0]["L42"].ToString();
                    lblDataLabel43.Text = dt.Rows[0]["L43"].ToString();
                    lblDataLabel44.Text = dt.Rows[0]["L44"].ToString();
                    lblDataLabel45.Text = dt.Rows[0]["L45"].ToString();
                    lblDataLabel46.Text = dt.Rows[0]["L46"].ToString();
                    lblDataLabel47.Text = dt.Rows[0]["L47"].ToString();
                    lblDataLabel48.Text = dt.Rows[0]["L48"].ToString();
                    lblDataLabel49.Text = dt.Rows[0]["L49"].ToString();
                    lblDataLabel50.Text = dt.Rows[0]["L50"].ToString();
                    lblDataLabel51.Text = dt.Rows[0]["L51"].ToString();
                    lblDataLabel52.Text = dt.Rows[0]["L52"].ToString();
                    lblDataLabel53.Text = dt.Rows[0]["L53"].ToString();
                    lblDataLabel54.Text = dt.Rows[0]["L54"].ToString();
                    lblDataLabel55.Text = dt.Rows[0]["L55"].ToString();
                    lblDataLabel56.Text = dt.Rows[0]["L56"].ToString();
                    lblDataLabel57.Text = dt.Rows[0]["L57"].ToString();
                    lblDataLabel58.Text = dt.Rows[0]["L58"].ToString();
                    lblDataLabel59.Text = dt.Rows[0]["L59"].ToString();
                    lblDataLabel60.Text = dt.Rows[0]["L60"].ToString();
                    lblDataLabel61.Text = dt.Rows[0]["L61"].ToString();
                    lblDataLabel62.Text = dt.Rows[0]["L62"].ToString();
                    lblDataLabel63.Text = dt.Rows[0]["L63"].ToString();
                    lblDataLabel64.Text = dt.Rows[0]["L64"].ToString();
                    lblDataLabel65.Text = dt.Rows[0]["L65"].ToString();
                    lblDataLabel66.Text = dt.Rows[0]["L66"].ToString();
                    lblDataLabel67.Text = dt.Rows[0]["L67"].ToString();
                    lblDataLabel68.Text = dt.Rows[0]["L68"].ToString();
                    lblDataLabel69.Text = dt.Rows[0]["L69"].ToString();
                    lblDataLabel70.Text = dt.Rows[0]["L70"].ToString();
                    lblDataLabel71.Text = dt.Rows[0]["L71"].ToString();
                    lblDataLabel72.Text = dt.Rows[0]["L72"].ToString();
                    lblDataLabel73.Text = dt.Rows[0]["L73"].ToString();
                    lblDataLabel74.Text = dt.Rows[0]["L74"].ToString();
                    lblDataLabel75.Text = dt.Rows[0]["L75"].ToString();
                    lblDataLabel76.Text = dt.Rows[0]["L76"].ToString();
                    lblDataLabel77.Text = dt.Rows[0]["L77"].ToString();
                    lblDataLabel78.Text = dt.Rows[0]["L78"].ToString();
                    lblDataLabel79.Text = dt.Rows[0]["L79"].ToString();
                    lblDataLabel80.Text = dt.Rows[0]["L80"].ToString();
                    lblDataLabel81.Text = dt.Rows[0]["L81"].ToString();
                    lblDataLabel82.Text = dt.Rows[0]["L82"].ToString();
                    lblDataLabel83.Text = dt.Rows[0]["L83"].ToString();
                    lblDataLabel84.Text = dt.Rows[0]["L84"].ToString();
                    lblDataLabel85.Text = dt.Rows[0]["L85"].ToString();
                    lblDataLabel86.Text = dt.Rows[0]["L86"].ToString();
                    lblDataLabel87.Text = dt.Rows[0]["L87"].ToString();
                    lblDataLabel88.Text = dt.Rows[0]["L88"].ToString();
                    lblDataLabel89.Text = dt.Rows[0]["L89"].ToString();
                    lblDataLabel90.Text = dt.Rows[0]["L90"].ToString();
                    lblDataLabel91.Text = dt.Rows[0]["L91"].ToString();
                    lblDataLabel92.Text = dt.Rows[0]["L92"].ToString();
                    lblDataLabel93.Text = dt.Rows[0]["L93"].ToString();
                    lblDataLabel94.Text = dt.Rows[0]["L94"].ToString();
                    lblDataLabel95.Text = dt.Rows[0]["L95"].ToString();
                    lblDataLabel96.Text = dt.Rows[0]["L96"].ToString();
                    lblDataLabel97.Text = dt.Rows[0]["L97"].ToString();
                    lblDataLabel98.Text = dt.Rows[0]["L98"].ToString();
                    lblDataLabel99.Text = dt.Rows[0]["L99"].ToString();
                    lblDataLabel100.Text = dt.Rows[0]["L100"].ToString();
                    lblDataLabel101.Text = dt.Rows[0]["L101"].ToString();
                    lblDataLabel102.Text = dt.Rows[0]["L102"].ToString();
                    lblDataLabel103.Text = dt.Rows[0]["L103"].ToString();
                    lblDataLabel104.Text = dt.Rows[0]["L104"].ToString();
                    lblDataLabel105.Text = dt.Rows[0]["L105"].ToString();
                    lblDataLabel106.Text = dt.Rows[0]["L106"].ToString();
                    lblDataLabel107.Text = dt.Rows[0]["L107"].ToString();
                    lblDataLabel108.Text = dt.Rows[0]["L108"].ToString();
                    lblDataLabel109.Text = dt.Rows[0]["L109"].ToString();
                    lblDataLabel110.Text = dt.Rows[0]["L110"].ToString();
                    lblDataLabel111.Text = dt.Rows[0]["L111"].ToString();
                    lblDataLabel112.Text = dt.Rows[0]["L112"].ToString();
                    lblDataLabel113.Text = dt.Rows[0]["L113"].ToString();
                    lblDataLabel114.Text = dt.Rows[0]["L114"].ToString();
                    lblDataLabel115.Text = dt.Rows[0]["L115"].ToString();
                    lblDataLabel116.Text = dt.Rows[0]["L116"].ToString();
                    lblDataLabel117.Text = dt.Rows[0]["L117"].ToString();
                    lblDataLabel118.Text = dt.Rows[0]["L118"].ToString();
                    lblDataLabel119.Text = dt.Rows[0]["L119"].ToString();
                    lblDataLabel120.Text = dt.Rows[0]["L120"].ToString();
                    lblDataLabel121.Text = dt.Rows[0]["L121"].ToString();
                    lblDataLabel122.Text = dt.Rows[0]["L122"].ToString();
                    lblDataLabel123.Text = dt.Rows[0]["L123"].ToString();
                    lblDataLabel124.Text = dt.Rows[0]["L124"].ToString();
                    lblDataLabel125.Text = dt.Rows[0]["L125"].ToString();
                    lblDataLabel126.Text = dt.Rows[0]["L126"].ToString();
                    lblDataLabel127.Text = dt.Rows[0]["L127"].ToString();
                    lblDataLabel128.Text = dt.Rows[0]["L128"].ToString();
                    lblDataLabel129.Text = dt.Rows[0]["L129"].ToString();
                    lblDataLabel130.Text = dt.Rows[0]["L130"].ToString();
                    lblDataLabel131.Text = dt.Rows[0]["L131"].ToString();
                    lblDataLabel132.Text = dt.Rows[0]["L132"].ToString();
                    lblDataLabel133.Text = dt.Rows[0]["L133"].ToString();
                    lblDataLabel134.Text = dt.Rows[0]["L134"].ToString();
                    lblDataLabel135.Text = dt.Rows[0]["L135"].ToString();
                    lblDataLabel136.Text = dt.Rows[0]["L136"].ToString();
                    lblDataLabel137.Text = dt.Rows[0]["L137"].ToString();
                    lblDataLabel138.Text = dt.Rows[0]["L138"].ToString();
                    lblDataLabel139.Text = dt.Rows[0]["L139"].ToString();
                    lblDataLabel140.Text = dt.Rows[0]["L140"].ToString();
                    lblDataLabel141.Text = dt.Rows[0]["L141"].ToString();
                    lblDataLabel142.Text = dt.Rows[0]["L142"].ToString();
                    lblDataLabel143.Text = dt.Rows[0]["L143"].ToString();
                    lblDataLabel144.Text = dt.Rows[0]["L144"].ToString();
                    lblDataLabel145.Text = dt.Rows[0]["L145"].ToString();
                    lblDataLabel146.Text = dt.Rows[0]["L146"].ToString();
                    lblDataLabel147.Text = dt.Rows[0]["L147"].ToString();
                    lblDataLabel148.Text = dt.Rows[0]["L148"].ToString();
                    lblDataLabel149.Text = dt.Rows[0]["L149"].ToString();
                    lblDataLabel150.Text = dt.Rows[0]["L150"].ToString();
                    lblDataLabel151.Text = dt.Rows[0]["L151"].ToString();
                    lblDataLabel152.Text = dt.Rows[0]["L152"].ToString();
                    lblDataLabel153.Text = dt.Rows[0]["L153"].ToString();
                    lblDataLabel154.Text = dt.Rows[0]["L154"].ToString();
                    lblDataLabel155.Text = dt.Rows[0]["L155"].ToString();
                    lblDataLabel156.Text = dt.Rows[0]["L156"].ToString();
                    lblDataLabel157.Text = dt.Rows[0]["L157"].ToString();
                    lblDataLabel158.Text = dt.Rows[0]["L158"].ToString();
                    lblDataLabel159.Text = dt.Rows[0]["L159"].ToString();
                    lblDataLabel160.Text = dt.Rows[0]["L160"].ToString();
                    lblDataLabel161.Text = dt.Rows[0]["L161"].ToString();
                    lblDataLabel162.Text = dt.Rows[0]["L162"].ToString();
                    lblDataLabel163.Text = dt.Rows[0]["L163"].ToString();
                    lblDataLabel164.Text = dt.Rows[0]["L164"].ToString();
                    lblDataLabel165.Text = dt.Rows[0]["L165"].ToString();
                    lblDataLabel166.Text = dt.Rows[0]["L166"].ToString();
                    lblDataLabel167.Text = dt.Rows[0]["L167"].ToString();
                    lblDataLabel168.Text = dt.Rows[0]["L168"].ToString();
                    lblDataLabel169.Text = dt.Rows[0]["L169"].ToString();
                    lblDataLabel170.Text = dt.Rows[0]["L170"].ToString();
                    lblDataLabel171.Text = dt.Rows[0]["L171"].ToString();
                    lblDataLabel172.Text = dt.Rows[0]["L172"].ToString();
                    lblDataLabel173.Text = dt.Rows[0]["L173"].ToString();
                    lblDataLabel174.Text = dt.Rows[0]["L174"].ToString();
                    lblDataLabel175.Text = dt.Rows[0]["L175"].ToString();
                    lblDataLabel176.Text = dt.Rows[0]["L176"].ToString();
                    lblDataLabel177.Text = dt.Rows[0]["L177"].ToString();
                    lblDataLabel178.Text = dt.Rows[0]["L178"].ToString();
                    lblDataLabel179.Text = dt.Rows[0]["L179"].ToString();
                    lblDataLabel180.Text = dt.Rows[0]["L180"].ToString();
                    
                    if (dt.Rows[0]["SD1"].ToString() != "")
                        chkSD1.Text = dt.Rows[0]["SD1"].ToString();
                    else
                        chkSD1.Text = "--XX--";

                    if (dt.Rows[0]["SD2"].ToString() != "")
                        chkSD2.Text = dt.Rows[0]["SD2"].ToString();
                    else
                        chkSD2.Text = "--XX--";

                    if (dt.Rows[0]["SD3"].ToString() != "")
                        chkSD3.Text = dt.Rows[0]["SD3"].ToString();
                    else
                        chkSD3.Text = "--XX--";

                    if (dt.Rows[0]["SD4"].ToString() != "")
                        chkSD4.Text = dt.Rows[0]["SD4"].ToString();
                    else
                        chkSD4.Text = "--XX--";
                    txtTableRptID.Text = dt.Rows[0]["TableReportID"].ToString();
                    lblDescription.Text = dt.Rows[0]["TableDesc"].ToString();
                }
                catch { }
            }
            if (chkSD1.Text == "--XX--")
                chkSD1.Enabled = false;
            if (chkSD2.Text == "--XX--")
                chkSD2.Enabled = false;
            if (chkSD3.Text == "--XX--")
                chkSD3.Enabled = false;
            if (chkSD4.Text == "--XX--")
                chkSD4.Enabled = false;
        }

        private void SetUpDGV()
        {
            dgvCol1.Rows.Clear(); dgvCol1.Columns.Clear();
            dgvCol2.Rows.Clear(); dgvCol2.Columns.Clear();
            dgvCol3.Rows.Clear(); dgvCol3.Columns.Clear();
            dgvCol4.Rows.Clear(); dgvCol4.Columns.Clear();
            dgvCol5.Rows.Clear(); dgvCol5.Columns.Clear();
            dgvCol6.Rows.Clear(); dgvCol6.Columns.Clear();

            dgvCol1.RowCount = 15; dgvCol1.ColumnCount = 2;  
            dgvCol2.RowCount = 15; dgvCol2.ColumnCount = 2;
            dgvCol3.RowCount = 15; dgvCol3.ColumnCount = 2;
            dgvCol4.RowCount = 15; dgvCol4.ColumnCount = 2;
            dgvCol5.RowCount = 15; dgvCol5.ColumnCount = 2;
            dgvCol6.RowCount = 15; dgvCol6.ColumnCount = 2;

            dgvCol1.Columns[0].Width = 111; dgvCol1.Columns[1].Width = 196; dgvCol1.Enabled = true;
            dgvCol2.Columns[0].Width = 111; dgvCol2.Columns[1].Width = 196; dgvCol2.Enabled = true;
            dgvCol3.Columns[0].Width = 111; dgvCol3.Columns[1].Width = 196; dgvCol3.Enabled = true;
            dgvCol4.Columns[0].Width = 111; dgvCol4.Columns[1].Width = 196; dgvCol4.Enabled = true;
            dgvCol5.Columns[0].Width = 111; dgvCol5.Columns[1].Width = 196; dgvCol5.Enabled = true;
            dgvCol6.Columns[0].Width = 111; dgvCol6.Columns[1].Width = 196; dgvCol6.Enabled = true;

            dgvCol1.Columns[0].DefaultCellStyle.BackColor = Color.Beige;
            dgvCol2.Columns[0].DefaultCellStyle.BackColor = Color.Beige;
            dgvCol3.Columns[0].DefaultCellStyle.BackColor = Color.Beige;
            dgvCol4.Columns[0].DefaultCellStyle.BackColor = Color.Beige;
            dgvCol5.Columns[0].DefaultCellStyle.BackColor = Color.Beige;
            dgvCol6.Columns[0].DefaultCellStyle.BackColor = Color.Beige;

            dgvCol1.ClearSelection(); dgvCol2.ClearSelection(); dgvCol3.ClearSelection();
            dgvCol4.ClearSelection(); dgvCol5.ClearSelection(); dgvCol6.ClearSelection();

            dgvCol1.CurrentCell = dgvCol1.Rows[0].Cells[1];
            
        }

        private void dgvCol1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (e.ColumnIndex == 1 && nMode == 0))
                e.Cancel = true;
        }

        private void dgvCol2_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (e.ColumnIndex == 1 && nMode == 0))
                e.Cancel = true;
        }

        private void dgvCol3_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (e.ColumnIndex == 1 && nMode == 0))
                e.Cancel = true;
        }

        private void dgvCol4_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (e.ColumnIndex == 1 && nMode == 0))
                e.Cancel = true;
        }

        private void dgvCol5_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (e.ColumnIndex == 1 && nMode == 0))
                e.Cancel = true;
        }

        private void dgvCol6_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 0 || (e.ColumnIndex == 1 && nMode == 0))
                e.Cancel = true;
        }

        private void dgvCol1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvCol1.CurrentRow.Index == 14)
                {
                    dgvCol1.ClearSelection();
                    dgvCol2.Select();
                    dgvCol2.CurrentCell = dgvCol2.Rows[0].Cells[1];
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (dgvCol1.CurrentRow.Index == 0)
                {
                    dgvCol1.ClearSelection();
                    dgvCol3.Select();
                    dgvCol3.CurrentCell = dgvCol3.Rows[14].Cells[1];
                }
            }
        }

        private void dgvCol2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvCol2.CurrentRow.Index == 14)
                {
                    dgvCol2.ClearSelection();
                    dgvCol3.Select();
                    dgvCol3.CurrentCell = dgvCol3.Rows[0].Cells[1];
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (dgvCol2.CurrentRow.Index == 0)
                {
                    dgvCol2.ClearSelection();
                    dgvCol1.Select();
                    dgvCol1.CurrentCell = dgvCol1.Rows[14].Cells[1];
                }
            }
        }

        private void dgvCol3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvCol3.CurrentRow.Index == 14)
                {
                    dgvCol3.ClearSelection();
                    dgvCol1.Select();
                    dgvCol1.CurrentCell = dgvCol1.Rows[0].Cells[1];
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (dgvCol3.CurrentRow.Index == 0)
                {
                    dgvCol3.ClearSelection();
                    dgvCol2.Select();
                    dgvCol2.CurrentCell = dgvCol2.Rows[14].Cells[1];
                }
            }
        }

        private void dgvCol4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvCol4.CurrentRow.Index == 14)
                {
                    dgvCol4.ClearSelection();
                    dgvCol5.Select();
                    dgvCol5.CurrentCell = dgvCol5.Rows[0].Cells[1];
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (dgvCol4.CurrentRow.Index == 0)
                {
                    dgvCol4.ClearSelection();
                    dgvCol6.Select();
                    dgvCol6.CurrentCell = dgvCol6.Rows[14].Cells[1];
                }
            }
        }

        private void dgvCol5_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvCol5.CurrentRow.Index == 14)
                {
                    dgvCol5.ClearSelection();
                    dgvCol6.Select();
                    dgvCol6.CurrentCell = dgvCol6.Rows[0].Cells[1];
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (dgvCol5.CurrentRow.Index == 0)
                {
                    dgvCol5.ClearSelection();
                    dgvCol4.Select();
                    dgvCol4.CurrentCell = dgvCol4.Rows[14].Cells[1];
                }
            }
        }

        private void dgvCol6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (dgvCol6.CurrentRow.Index == 14)
                {
                    dgvCol6.ClearSelection();
                    dgvCol4.Select();
                    dgvCol4.CurrentCell = dgvCol4.Rows[0].Cells[1];
                }
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (dgvCol6.CurrentRow.Index == 0)
                {
                    dgvCol6.ClearSelection();
                    dgvCol5.Select();
                    dgvCol5.CurrentCell = dgvCol5.Rows[14].Cells[1];
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bsMain.EndEdit();
            DataTable dt = new DataTable();
            dt = dtMain.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();

                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SC", nServiceCode);
                sqlcmd.Parameters.AddWithValue("@DataFormat", cboTableFormats.Text);

                if (txtExptNo.Text.Trim() != "")
                    sqlcmd.Parameters.AddWithValue("@ExpNo", Convert.ToInt32(txtExptNo.Text));
                else
                    sqlcmd.Parameters.AddWithValue("@ExpNo", DBNull.Value);
                if (txtPageNo.Text.Trim() != "")
                    sqlcmd.Parameters.AddWithValue("@PageNo", Convert.ToInt32(txtPageNo.Text));
                else
                    sqlcmd.Parameters.AddWithValue("@PageNo", DBNull.Value);
                if (cboAnalysts.SelectedIndex != 0)
                    sqlcmd.Parameters.AddWithValue("@AnalystID", cboAnalysts.SelectedValue);
                else
                    sqlcmd.Parameters.AddWithValue("@AnalystID", DBNull.Value);

                sqlcmd.Parameters.AddWithValue("@Media", txtMedia.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spUpdLogTest";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sqlcnn.Dispose();
                    return;
                }
                sqlcmd.Dispose();
                sqlcnn.Close(); sqlcnn.Dispose();
                dtMain.AcceptChanges();
            }

            bsData.EndEdit();
            string strTestData = "";
            dt = new DataTable();
            dt = dtSampleSC.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SqlCommand sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;

                    strTestData = "<TestData>";
                    for (int j = 1; j <= 180; j++)
                    {
                        if (dt.Rows[i]["V" + j.ToString()].ToString().Trim() != null && dt.Rows[i]["V" + j.ToString()].ToString().Trim() != "")
                        {
                            //Escape
                            string strX = dt.Rows[i]["V" + j.ToString()].ToString().Trim();
                            strX = strX.Replace("&", "&amp;");
                            strX = strX.Replace(">", "&gt;");
                            strX = strX.Replace("<", "&lt;");
                            strX = strX.Replace("'", "&apos;");
                            strX = strX.Replace("\"", "&quot;");
                            strTestData = strTestData + "<Value" + j.ToString() + ">" + strX + "</Value" + j.ToString() + ">";
                        }
                    }
                    if (dt.Rows[i]["Notes"] != null && dt.Rows[i]["Notes"].ToString().Trim() != "")
                    {
                        string strN = dt.Rows[i]["Notes"].ToString().Trim();
                        strN = strN.Replace("&", "&amp;");
                        strN = strN.Replace(">", "&gt;");
                        strN = strN.Replace("<", "&lt;");
                        strN = strN.Replace("'", "&apos;");
                        strN = strN.Replace("\"", "&quot;");
                        strTestData = strTestData + "<Note>" + strN + "</Note>";
                    }
                    if (dt.Rows[i]["SD1"] != null && dt.Rows[i]["SD1"].ToString().Trim() != "")
                        strTestData = strTestData + "<SD1>" + Convert.ToByte(dt.Rows[i]["SD1"]).ToString() + "</SD1>";
                    if (dt.Rows[i]["SD2"] != null && dt.Rows[i]["SD2"].ToString().Trim() != "")
                        strTestData = strTestData + "<SD2>" + Convert.ToByte(dt.Rows[i]["SD2"]).ToString() + "</SD2>";
                    if (dt.Rows[i]["SD3"] != null && dt.Rows[i]["SD3"].ToString().Trim() != "")
                        strTestData = strTestData + "<SD3>" + Convert.ToByte(dt.Rows[i]["SD3"]).ToString() + "</SD3>";
                    if (dt.Rows[i]["SD4"] != null && dt.Rows[i]["SD4"].ToString().Trim() != "")
                        strTestData = strTestData + "<SD4>" + Convert.ToByte(dt.Rows[i]["SD4"]).ToString() + "</SD4>";
                    strTestData = strTestData + "</TestData>";
                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;

                    sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                    sqlcmd.Parameters.AddWithValue("@SC", nServiceCode);
                    sqlcmd.Parameters.AddWithValue("@SlashNo", dt.Rows[i]["SlashNo"].ToString());
                    sqlcmd.Parameters.AddWithValue("@TestDataValues", strTestData);
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spUpdTestDataValues";
                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                    sqlcmd.Dispose();
                }
                dtSampleSC.AcceptChanges();
                sqlcnn.Close(); sqlcnn.Dispose();
                //Implementation of Final Report Details - AMDC 11/10/2017
                //if (txtReportNo.Text != "")
                //{
                //    string strRptDtls = "<ExtendedData><ReportData>" +
                //                        "<GBLNo>" + txtGBLNo.Text + "</GBLNo>" +
                //                        "<SC>" + txtSC.Text + "</SC>" +
                //                        "</ReportData></ExtendedData>";
                //    PSSClass.FinalReports.UpdFinRptDtls(Convert.ToInt32(txtReportNo.Text), strRptDtls);
                //}
            }
            this.Dispose();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            OpenControls(this, true);
        }

        private void btnPrintPreview_Click(object sender, EventArgs e)
        {
            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            SqlCommand sqlcmd = new SqlCommand();

            //Save
            bsMain.EndEdit();
            DataTable dt = new DataTable();
            dt = dtMain.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                sqlcmd.Connection = sqlcnn;

                sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                sqlcmd.Parameters.AddWithValue("@SC", nServiceCode);
                sqlcmd.Parameters.AddWithValue("@DataFormat", cboTableFormats.Text);
                if (txtExptNo.Text.Trim() != "")
                    sqlcmd.Parameters.AddWithValue("@ExpNo", Convert.ToInt32(txtExptNo.Text));
                else
                    sqlcmd.Parameters.AddWithValue("@ExpNo", DBNull.Value);
                if (txtPageNo.Text.Trim() != "")
                    sqlcmd.Parameters.AddWithValue("@PageNo", Convert.ToInt32(txtPageNo.Text));
                else
                    sqlcmd.Parameters.AddWithValue("@PageNo", DBNull.Value);

                sqlcmd.Parameters.AddWithValue("@AnalystID", cboAnalysts.SelectedValue);
                sqlcmd.Parameters.AddWithValue("@Media", txtMedia.Text.Trim());
                sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.CommandText = "spUpdLogTest";
                try
                {
                    sqlcmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    sqlcnn.Dispose();
                    return;
                }
                sqlcmd.Dispose();
                sqlcnn.Close(); sqlcnn.Dispose();
                dtMain.AcceptChanges();
            }

            bsData.EndEdit();
            string strTestData = "";
            dt = new DataTable();
            dt = dtSampleSC.GetChanges(DataRowState.Modified);
            if (dt != null)
            {
                sqlcnn = PSSClass.DBConnection.PSSConnection();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;

                    strTestData = "<TestData>";
                    for (int j = 1; j <= 180; j++)
                    {
                        if (dt.Rows[i]["V" + j.ToString()].ToString().Trim() != null && dt.Rows[i]["V" + j.ToString()].ToString().Trim() != "")
                        {
                            //Escape
                            string strX = dt.Rows[i]["V" + j.ToString()].ToString().Trim();
                            strX = strX.Replace("&", "&amp;");
                            strX = strX.Replace(">", "&gt;");
                            strX = strX.Replace("<", "&lt;");
                            strX = strX.Replace("'", "&apos;");
                            strX = strX.Replace("\"", "&quot;");
                            strTestData = strTestData + "<Value" + j.ToString() + ">" + strX + "</Value" + j.ToString() + ">";
                        }
                    }
                    if (dt.Rows[i]["Notes"] != null && dt.Rows[i]["Notes"].ToString().Trim() != "")
                    {
                        string strN = dt.Rows[i]["Notes"].ToString().Trim();
                        strN = strN.Replace("&", "&amp;");
                        strN = strN.Replace(">", "&gt;");
                        strN = strN.Replace("<", "&lt;");
                        strN = strN.Replace("'", "&apos;");
                        strN = strN.Replace("\"", "&quot;");
                        strTestData = strTestData + "<Note>" + strN + "</Note>";
                    }
                    if (dt.Rows[i]["SD1"] != null && dt.Rows[i]["SD1"].ToString().Trim() != "")
                        strTestData = strTestData + "<SD1>" + Convert.ToByte(dt.Rows[i]["SD1"]).ToString() + "</SD1>";
                    if (dt.Rows[i]["SD2"] != null && dt.Rows[i]["SD2"].ToString().Trim() != "")
                        strTestData = strTestData + "<SD2>" + Convert.ToByte(dt.Rows[i]["SD2"]).ToString() + "</SD2>";
                    if (dt.Rows[i]["SD3"] != null && dt.Rows[i]["SD3"].ToString().Trim() != "")
                        strTestData = strTestData + "<SD3>" + Convert.ToByte(dt.Rows[i]["SD3"]).ToString() + "</SD3>";
                    if (dt.Rows[i]["SD4"] != null && dt.Rows[i]["SD4"].ToString().Trim() != "")
                        strTestData = strTestData + "<SD4>" + Convert.ToByte(dt.Rows[i]["SD4"]).ToString() + "</SD4>";
                    strTestData = strTestData + "</TestData>";
                    sqlcmd = new SqlCommand();
                    sqlcmd.Connection = sqlcnn;

                    sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
                    sqlcmd.Parameters.AddWithValue("@SC", nServiceCode);
                    sqlcmd.Parameters.AddWithValue("@SlashNo", dt.Rows[i]["SlashNo"].ToString());
                    sqlcmd.Parameters.AddWithValue("@TestDataValues", strTestData);
                    sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.CommandText = "spUpdTestDataValues";
                    try
                    {
                        sqlcmd.ExecuteNonQuery();
                    }
                    catch
                    {
                    }
                    sqlcmd.Dispose();
                }
                dtSampleSC.AcceptChanges();
                //Implementation of Final Report Details - AMDC 11/10/2017
                //if (txtReportNo.Text != "")
                //{
                //    string strRptDtls = "<ExtendedData><ReportData>" +
                //                        "<GBLNo>" + txtGBLNo.Text + "</GBLNo>" +
                //                        "<SC>" + txtSC.Text + "</SC>" +
                //                        "</ReportData></ExtendedData>";
                //    PSSClass.FinalReports.UpdFinRptDtls(Convert.ToInt32(txtReportNo.Text), strRptDtls);
                //}
            }

            //Update Data Format
            sqlcmd = new SqlCommand();
            sqlcmd.Connection = sqlcnn;

            sqlcmd.Parameters.AddWithValue("@LogNo", nLogNo);
            sqlcmd.Parameters.AddWithValue("@SC", nServiceCode);
            sqlcmd.Parameters.AddWithValue("@DataFormat", cboTableFormats.Text);
            sqlcmd.Parameters.AddWithValue("@UserID", LogIn.nUserID);

            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.CommandText = "spUpdDataFormat";
            try
            {
                sqlcmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error encountered: " + ex.Message + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                sqlcnn.Dispose();
                return;
            }
            sqlcmd.Dispose(); 
            sqlcnn.Close(); sqlcnn.Dispose();

            if (File.Exists(@"\\PSAPP01\IT Files\PTS\Crystal Reports\" + txtTableRptID.Text.Replace(".rpt", "") + ".rpt") == false)
            {
                MessageBox.Show("Report file is under construction." + Environment.NewLine + "Please contact the IT Department for updates.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            LabRpt rpt = new LabRpt();
            rpt.rptName = "SpeedReport";
            rpt.rptFile = txtTableRptID.Text + ".rpt";
            rpt.WindowState = FormWindowState.Maximized;
            rpt.nLogNo = Convert.ToInt32(txtGBLNo.Text);
            rpt.nSC = Convert.ToInt32(txtSC.Text);
            rpt.SpID = Convert.ToInt16(txtSpID.Text);
            rpt.nFormat = Convert.ToInt16(cboTableFormats.Text);

            try
            {
                rpt.Show();
            }
            catch { }
        }

        private void dgvCol1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            txtSlash.Text = txtSlashNo.Text;
            nEdit = 1;
        }

        private void dgvCol2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            txtSlash.Text = txtSlashNo.Text;
            nEdit = 1;
        }

        private void dgvCol3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            txtSlash.Text = txtSlashNo.Text;
            nEdit = 1;
        }

        private void dgvCol4_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            txtSlash.Text = txtSlashNo.Text;
            nEdit = 1;
        }

        private void dgvCol5_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            txtSlash.Text = txtSlashNo.Text;
            nEdit = 1;
        }

        private void dgvCol6_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            txtSlash.Text = txtSlashNo.Text;
            nEdit = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bsMain.EndEdit();
            MessageBox.Show(dtMain.Rows[0].RowState.ToString());

            bsData.EndEdit();
            for (int i = 0; i < dtSampleSC.Rows.Count; i++)
            {
                MessageBox.Show(dtSampleSC.Rows[i].RowState.ToString());
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("This process copies current slash data to other slashes.", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (dReply == DialogResult.OK)
            {
                DialogResult dAnswer = new DialogResult();
                dAnswer = MessageBox.Show("WARNING: This would overwrite existing data on target slashes." + Environment.NewLine + "Are you sure you want to do this?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dAnswer == DialogResult.Yes)
                {
                    int n = bsData.Position;
                    for (int i = n; i < dtSampleSC.Rows.Count; i++)
                    {
                        for (int j = 1; j <= 45; j++)
                        {
                            dtSampleSC.Rows[i]["V" + j.ToString()] = ((TextBox)tabPage1.Controls["txtDataValue" + j.ToString().Trim()]).Text;
                        }
                        for (int j = 46; j <= 90; j++)
                        {
                            dtSampleSC.Rows[i]["V" + j.ToString()] = ((TextBox)tabPage2.Controls["txtDataValue" + j.ToString().Trim()]).Text;
                        }
                    }
                }
            }
        }

        private void btnPasteData_Click(object sender, EventArgs e)
        {
            DialogResult dAnswer = new DialogResult();
            dAnswer = MessageBox.Show("WARNING: This would overwrite existing data on this table." + Environment.NewLine + "Are you sure you want to do this?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dAnswer == DialogResult.No)
            {
                return;
            }
            string strX = "";
            string strXML = Application.StartupPath + "\\" + txtSpID.Text + "_" + txtSC.Text + "_" + cboTableFormats.Text + "_" + ".xml";
            DataSet dsXML = new DataSet();
            DataView dvwXML;
            try
            {
                dsXML.ReadXml(strXML);
            }
            catch
            {
                MessageBox.Show("No matching data in memory.");
                return;
            }
            dvwXML = new DataView(dsXML.Tables[0]);
            cboTableFormats.Text = dvwXML[0].Row["TableFormat"].ToString();
            for (int i = 0; i < 90; i++)
            {
                if (i < 45)
                {
                    strX = dvwXML[0].Row["Value" + (i + 1).ToString()].ToString();
                    ((TextBox)tabPage1.Controls["txtDataValue" + (i + 1).ToString().Trim()]).Text = strX;
                }
                else
                {
                    strX = dvwXML[0].Row["Value" + (i + 1).ToString()].ToString();
                    ((TextBox)tabPage2.Controls["txtDataValue" + (i + 1).ToString().Trim()]).Text = strX;
                }
            }
            if (dvwXML[0].Row["SD1"].ToString() == "1")
                chkSD1.Checked = true;
            else
                chkSD1.Checked = false;

            if (dvwXML[0].Row["SD2"].ToString() == "1")
                chkSD2.Checked = true;
            else
                chkSD2.Checked = false;

            if (dvwXML[0].Row["SD3"].ToString() == "1")
                chkSD3.Checked = true;
            else
                chkSD3.Checked = false;

            if (dvwXML[0].Row["SD4"].ToString() == "1")
                chkSD4.Checked = true;
            else
                chkSD4.Checked = false;

            txtNote.Text = dvwXML[0].Row["Note"].ToString();

            dsXML.Clear(); dsXML.Dispose(); dvwXML.Dispose();
        }

        private void btnCopyData_Click(object sender, EventArgs e)
        {
            string strTestData = "<TestData>";

            string strEsc = cboTableFormats.Text.Trim();
            strEsc = strEsc.Replace("&", "&amp;");
            strEsc = strEsc.Replace(">", "&gt;");
            strEsc = strEsc.Replace("<", "&lt;");
            strEsc = strEsc.Replace("'", "&apos;");
            strEsc = strEsc.Replace("\"", "&quot;");
            strTestData = strTestData + "<TableFormat>" + strEsc + "</TableFormat>";

            for (int i = 0; i < 90; i++)
            {
                strTestData = strTestData + "<Value" + (i + 1).ToString() + ">";
                if (i < 45)
                {
                    //Escape
                    string strX = ((TextBox)tabPage1.Controls["txtDataValue" + (i + 1).ToString().Trim()]).Text;
                    strX = strX.Replace("&", "&amp;");
                    strX = strX.Replace(">", "&gt;");
                    strX = strX.Replace("<", "&lt;");
                    strX = strX.Replace("'", "&apos;");
                    strX = strX.Replace("\"", "&quot;");
                    strTestData = strTestData + strX;
                }
                else
                {
                    string strX = ((TextBox)tabPage2.Controls["txtDataValue" + (i + 1).ToString().Trim()]).Text;
                    strX = strX.Replace("&", "&amp;");
                    strX = strX.Replace(">", "&gt;");
                    strX = strX.Replace("<", "&lt;");
                    strX = strX.Replace("'", "&apos;");
                    strX = strX.Replace("\"", "&quot;");
                    strTestData = strTestData + strX;
                }
                strTestData = strTestData + "</Value" + (i + 1).ToString() + ">";
            }

            for (int i = 0; i < 90; i++)
            {
                strTestData = strTestData + "<Value" + (i + 91).ToString() + ">";
                if (i < 45)
                {
                    //Escape
                    string strX = ((TextBox)tabPage3.Controls["txtDataValue" + (i + 91).ToString().Trim()]).Text;
                    strX = strX.Replace("&", "&amp;");
                    strX = strX.Replace(">", "&gt;");
                    strX = strX.Replace("<", "&lt;");
                    strX = strX.Replace("'", "&apos;");
                    strX = strX.Replace("\"", "&quot;");
                    strTestData = strTestData + strX;
                }
                else
                {
                    string strX = ((TextBox)tabPage4.Controls["txtDataValue" + (i + 91).ToString().Trim()]).Text;
                    strX = strX.Replace("&", "&amp;");
                    strX = strX.Replace(">", "&gt;");
                    strX = strX.Replace("<", "&lt;");
                    strX = strX.Replace("'", "&apos;");
                    strX = strX.Replace("\"", "&quot;");
                    strTestData = strTestData + strX;
                }
                strTestData = strTestData + "</Value" + (i + 91).ToString() + ">";
            }


            strEsc = txtNote.Text.Trim();
            strEsc = strEsc.Replace("&", "&amp;");
            strEsc = strEsc.Replace(">", "&gt;");
            strEsc = strEsc.Replace("<", "&lt;");
            strEsc = strEsc.Replace("'", "&apos;");
            strEsc = strEsc.Replace("\"", "&quot;");
            strTestData = strTestData + "<Note>" + strEsc + "</Note>";

            if (chkSD1.Checked == true)
                strTestData = strTestData + "<SD1>1</SD1>";
            else
                strTestData = strTestData + "<SD1>0</SD1>";

            if (chkSD2.Checked == true)
                strTestData = strTestData + "<SD2>1</SD2>";
            else
                strTestData = strTestData + "<SD2>0</SD2>";

            if (chkSD3.Checked == true)
                strTestData = strTestData + "<SD3>1</SD3>";
            else
                strTestData = strTestData + "<SD3>0</SD3>";

            if (chkSD4.Checked == true)
                strTestData = strTestData + "<SD4>1</SD4>";
            else
                strTestData = strTestData + "<SD4>0</SD4>";

            strTestData = strTestData + "</TestData>";
            File.WriteAllText(Application.StartupPath + "\\" + txtSpID.Text + "_" + txtSC.Text + "_" + cboTableFormats.Text + "_" + ".xml", strTestData, Encoding.ASCII);
            MessageBox.Show("Test dataset is copied successfully");
        }

        private void btnEMail_Click(object sender, EventArgs e)
        {
            string strRptName = "";
            DialogResult dReply = new DialogResult();
            dReply = MessageBox.Show("Do you want to send this to the Sponsor?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dReply == DialogResult.No)
            {
                return;
            }
            if (txtSC.Text == "297")
            {
                strRptName = "SpeedRpt_297.rpt";
            }
            else
            {
                if (File.Exists(@"\\GBLNJ4\GIS\Reports\" + txtTableRptID.Text.Replace(".rpt", "") + ".rpt") == false)
                {
                    MessageBox.Show("Report file is under construction." + Environment.NewLine + "Please contact the IT Department for updates.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                strRptName = txtTableRptID.Text + ".rpt";
            }
            LabRpt rpt = new LabRpt();
            rpt.rptName = "SpeedReport";
            rpt.rptFile = strRptName;
            rpt.WindowState = FormWindowState.Maximized;
            rpt.nLogNo = Convert.ToInt32(txtGBLNo.Text);
            rpt.nSC = Convert.ToInt32(txtSC.Text);
            rpt.SpID = Convert.ToInt16(txtSpID.Text);

            try
            {
                rpt.Show();
            }
            catch { }

            string pdfFile = @"\\gblnj4\GIS\Reports\SR-" + txtGBLNo.Text + "-" + txtSC.Text + ".pdf";
            int nConID = PSSClass.Samples.LogContactID (Convert.ToInt32(txtGBLNo.Text));

            string strText = "", strEMail = "";
            string strCFName = "";
            string strSignature = ReadSignature();

            SqlConnection sqlcnn = PSSClass.DBConnection.PSSConnection();
            if (sqlcnn == null)
            {
                MessageBox.Show("Connection problems encountered." + Environment.NewLine + "Please contact the IT Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataReader sqldr;

            sqlcmd = new SqlCommand("SELECT FirstName FROM Contacts WHERE ContactID = " + nConID, sqlcnn);
            sqldr = sqlcmd.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                strCFName = sqldr.GetValue(0).ToString();
            }
            sqldr.Close(); sqlcmd.Dispose();

            sqlcmd = new SqlCommand("SELECT EMailAddress FROM ContactEMAddresses WHERE ContactID = " + nConID.ToString()  +
                                    " AND SpeedReports = 1", sqlcnn);
            sqldr = sqlcmd.ExecuteReader();
            if (sqldr.HasRows)
            {
                sqldr.Read();
                strEMail = sqldr.GetValue(0).ToString();
                strEMail.Replace(";", ",");
            }
            sqldr.Close(); sqlcmd.Dispose();
            if (strEMail.Trim() == "")
            {
                MessageBox.Show("No e-mail settings found." + Environment.NewLine + "Please contact Technical Services Department.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            sqlcnn.Close(); sqlcnn.Dispose();
            Outlook.Application oApp = new Outlook.Application();
            // Create a new mail item.

            Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

            oMsg.HTMLBody = "<FONT face=\"Arial\">";
            // Set HTMLBody. 
            //add the body of the email
            strText = "Dear " + strCFName + ", <br/><br/>" +
                "For your convenience attached you will find our Speed Report™ on " + PSSClass.ServiceCodes.SCDescription(Convert.ToInt16(txtSC.Text)) + ".<br/><br/>" ;

            oMsg.HTMLBody += strText.Trim() + strSignature;

            //Add an attachment.
            oMsg.Attachments.Add(pdfFile);
            //oMsg.Attachments.Add(crafFile);
            //Subject line
            oMsg.Subject = "Article: " + PSSClass.Samples.ArticleDesc(pubCmpy, Convert.ToInt32(txtGBLNo.Text));
            // Add a recipient.
            Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;

            string[] EMAddresses = strEMail.Split(";,".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < EMAddresses.Length; i++)
            {
                if (EMAddresses[i].Trim() != "")
                {
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(EMAddresses[i]);
                    oRecip.Resolve();
                }
            }
            //oRecip.Resolve();
            oMsg.Display();

            // Send.
            //oMsg.Send();
            //((Outlook._MailItem)oMsg).Send();
            // Clean up.
            //oRecip = null;
            oRecips = null;
            oMsg = null;
            oApp = null;
        }

        private string ReadSignature()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
            string signature = string.Empty;
            DirectoryInfo diInfo = new DirectoryInfo(appDataDir);
            if (diInfo.Exists)
            {
                FileInfo[] fiSignature = diInfo.GetFiles("*.htm");
                if (fiSignature.Length > 0)
                {
                    StreamReader sr = new StreamReader(fiSignature[0].FullName, Encoding.Default);
                    signature = sr.ReadToEnd();

                    if (!string.IsNullOrEmpty(signature))
                    {
                        string fileName = fiSignature[0].Name.Replace(fiSignature[0].Extension, string.Empty);
                        signature = signature.Replace(fileName + "_files/", appDataDir + "/" + fileName + "_files/");
                    }
                }
            }
            return signature;
        }

        private void TestDataValues_Activated(object sender, EventArgs e)
        {
            string strFileAccess = PSSClass.General.UserFileAccess(LogIn.nUserID, "TestDataValues");

            if (nRptNo != 0)
            {
                txtReportNo.Text = nRptNo.ToString();
                DataTable dt = PSSClass.FinalReports.FinRptStatus(nRptNo);

                if (dt == null || dt.Rows.Count == 0)
                {
                    if (dtSampleSC.Rows.Count > 1)
                    {
                        btnCopy.Enabled = true; btnPasteData.Enabled = true;
                    }
                    else
                    {
                        btnCopy.Enabled = false; btnPasteData.Enabled = false;
                    }
                    if (strFileAccess == "RO")
                    {
                        OpenControls(this, false); OpenControls(tabPage1, false); OpenControls(tabPage2, false); OpenControls(tabPage3, false); OpenControls(tabPage4, false);
                        btnCopyData.Enabled = false; btnPasteData.Enabled = false; btnCopy.Enabled = false; btnPrintPreview.Enabled = false; btnEMail.Enabled = false;
                    }
                    else if (strFileAccess == "RW" || strFileAccess == "FA")
                    {
                        OpenControls(this, true); OpenControls(tabPage1, true); OpenControls(tabPage2, true); OpenControls(tabPage3, true); OpenControls(tabPage4, true);
                        btnCopyData.Enabled = true; btnPasteData.Enabled = true; btnCopy.Enabled = true; btnPrintPreview.Enabled = true;
                        if (txtSpID.Text == "3058" || txtSpID.Text == "1345" || txtSpID.Text == "2974" || txtSpID.Text == "1874" || txtSpID.Text == "3358") //txtSpID.Text != "3120" Test Sponsor
                            btnEMail.Enabled = true;
                        else
                            btnEMail.Enabled = false;
                    }
                }
                else
                {
                    if (strFileAccess == "RO")
                    {
                        OpenControls(this, false); OpenControls(tabPage1, false); OpenControls(tabPage2, false); OpenControls(tabPage3, false); OpenControls(tabPage4, false);
                        btnCopyData.Enabled = false; btnPasteData.Enabled = false; btnCopy.Enabled = false; btnPrintPreview.Enabled = false; btnEMail.Enabled = false;
                    }
                    else if (strFileAccess == "RW")
                    {
                        OpenControls(this, false); OpenControls(tabPage1, false); OpenControls(tabPage2, false); OpenControls(tabPage3, false); OpenControls(tabPage4, false);
                        btnCopyData.Enabled = true; btnPasteData.Enabled = false; btnCopy.Enabled = false; btnPrintPreview.Enabled = true; btnEMail.Enabled = false;
                    }
                    else if (strFileAccess == "FA")
                    {
                        OpenControls(this, false); OpenControls(tabPage1, false); OpenControls(tabPage2, false); OpenControls(tabPage3, false); OpenControls(tabPage4, false);
                        btnCopyData.Enabled = true; btnPasteData.Enabled = false; btnCopy.Enabled = false; btnPrintPreview.Enabled = true; btnEMail.Enabled = true;
                        if (txtSpID.Text == "3058" || txtSpID.Text == "1345" || txtSpID.Text == "2974" || txtSpID.Text == "1874" || txtSpID.Text == "3358")// txtSpID.Text != "3120" Test Sponsor
                            btnEMail.Enabled = false;
                        else
                            btnEMail.Enabled = true;
                    }
                }
            }
            else
            {
                if (dtSampleSC.Rows.Count > 1)
                    btnCopy.Enabled = true;
                else
                    btnCopy.Enabled = false;

                if (strFileAccess == "RO")
                {
                    OpenControls(this, false); OpenControls(tabPage1, false); OpenControls(tabPage2, false); OpenControls(tabPage3, false); OpenControls(tabPage4, false);
                    btnCopyData.Enabled = false; btnPasteData.Enabled = false; btnCopy.Enabled = false; btnPrintPreview.Enabled = false; btnEMail.Enabled = false;
                }
                else if (strFileAccess == "RW" || strFileAccess == "FA")
                {
                    OpenControls(this, true); OpenControls(tabPage1, true); OpenControls(tabPage2, true); OpenControls(tabPage3, true); OpenControls(tabPage4, true);
                    btnCopyData.Enabled = true; btnPasteData.Enabled = true; btnCopy.Enabled = true; btnPrintPreview.Enabled = true;
                    if (txtSpID.Text == "3058" || txtSpID.Text == "1345" || txtSpID.Text == "2974" || txtSpID.Text == "1874" || txtSpID.Text == "3358")
                        btnEMail.Enabled = true;
                    else
                        btnEMail.Enabled = false;
                }
            }
            OpenControls(pnlLogData, false);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TradingCardManager
{
    public partial class MainForm : Form
    {
        private DatabaseHelper dbHelper;
        private CollectionHelper collectionHelper;
        private Collection currentCollection;
        private string currentCollectionPath;
        private List<Card> currentSearchResults;
        private int totalCardCount = 0;
        private int currentPage = 0;
        private int pageSize = 100;
        private string currentExpansionFilter = "";
        private string currentColorFilter = "";
        private string currentRarityFilter = "";
        private string currentCardTypeFilter = "";
        private string currentLevelFilter = "";
        private string currentCostFilter = "";
        private string currentTriggerFilter = "";
        private string currentSideFilter = "";

        // Form controls
        private TabControl tabControl;
        private TabPage tabSearch;
        private TabPage tabCollection;

        private Panel pnlSearch;
        private Label lblSearchTerm;
        private TextBox txtSearchTerm;
        private Button btnSearch;
        private ComboBox cboSearchField;
        private Label lblSearchField;

        private ListView lvSearchResults;
        private ColumnHeader chName;
        private ColumnHeader chExpansion;
        private ColumnHeader chRarity;
        private ColumnHeader chColor;
        private ColumnHeader chCardType;

        // Paging controls
        private Panel pnlPagination;
        private Label lblResultCount;
        private Button btnPrevPage;
        private Button btnNextPage;
        private Label lblPageInfo;

        private Panel pnlSearchFilters;
        private Label lblExpansion;
        private ComboBox cboExpansion;
        private Label lblColor;
        private ComboBox cboColor;
        private Label lblRarity;
        private ComboBox cboRarity;
        private Label lblCardType;
        private ComboBox cboCardType;
        private Label lblLevel;
        private ComboBox cboLevel;
        private Label lblCost;
        private ComboBox cboCost;
        private Label lblTrigger;
        private ComboBox cboTrigger;
        private Label lblSide;
        private ComboBox cboSide;
        private Button btnApplyFilters;
        private Button btnClearFilters;

        private Panel pnlCardDetail;
        private Button btnAddToCollection;
        private Label lblCardDetail;
        private PictureBox pbCardImage;

        private ListView lvCollection;
        private ColumnHeader chColName;
        private ColumnHeader chColExpansion;
        private ColumnHeader chColRarity;
        private ColumnHeader chColColor;
        private ColumnHeader chColQuantity;

        private Panel pnlCollectionActions;
        private Button btnSaveCollection;
        private Button btnLoadCollection;
        private Label lblCollectionStats;
        private Label lblCollectionCount;

        private Panel pnlCollectionDetail;
        private Button btnRemoveFromCollection;
        private Label lblCollectionDetail;
        private PictureBox pbCollectionImage;

        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newCollectionToolStripMenuItem;
        private ToolStripMenuItem openCollectionToolStripMenuItem;
        private ToolStripMenuItem saveCollectionToolStripMenuItem;
        private ToolStripMenuItem saveCollectionAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem exitToolStripMenuItem;

        public MainForm()
        {
            InitializeComponent();
            string dbPath = Path.Combine(Application.StartupPath, "WSCards.db");
            dbHelper = new DatabaseHelper(dbPath);
            collectionHelper = new CollectionHelper(dbHelper);
            currentCollection = new Collection();
            currentSearchResults = new List<Card>();

            // Set the default collection path in Documents folder
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            currentCollectionPath = Path.Combine(documentsPath, "CardCollection.json");

            // Try to load existing collection
            if (File.Exists(currentCollectionPath))
            {
                currentCollection = collectionHelper.LoadCollection(currentCollectionPath);
                UpdateCollectionStats();
            }
        }

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabSearch = new System.Windows.Forms.TabPage();
            this.lvSearchResults = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chExpansion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chRarity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chCardType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.lblSearchTerm = new System.Windows.Forms.Label();
            this.txtSearchTerm = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cboSearchField = new System.Windows.Forms.ComboBox();
            this.lblSearchField = new System.Windows.Forms.Label();
            this.pnlPagination = new System.Windows.Forms.Panel();
            this.lblResultCount = new System.Windows.Forms.Label();
            this.btnPrevPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.pnlSearchFilters = new System.Windows.Forms.Panel();
            this.lblExpansion = new System.Windows.Forms.Label();
            this.cboExpansion = new System.Windows.Forms.ComboBox();
            this.lblColor = new System.Windows.Forms.Label();
            this.cboColor = new System.Windows.Forms.ComboBox();
            this.lblRarity = new System.Windows.Forms.Label();
            this.cboRarity = new System.Windows.Forms.ComboBox();
            this.lblCardType = new System.Windows.Forms.Label();
            this.cboCardType = new System.Windows.Forms.ComboBox();
            this.lblLevel = new System.Windows.Forms.Label();
            this.cboLevel = new System.Windows.Forms.ComboBox();
            this.lblCost = new System.Windows.Forms.Label();
            this.cboCost = new System.Windows.Forms.ComboBox();
            this.lblTrigger = new System.Windows.Forms.Label();
            this.cboTrigger = new System.Windows.Forms.ComboBox();
            this.lblSide = new System.Windows.Forms.Label();
            this.cboSide = new System.Windows.Forms.ComboBox();
            this.btnApplyFilters = new System.Windows.Forms.Button();
            this.btnClearFilters = new System.Windows.Forms.Button();
            this.pnlCardDetail = new System.Windows.Forms.Panel();
            this.btnAddToCollection = new System.Windows.Forms.Button();
            this.lblCardDetail = new System.Windows.Forms.Label();
            this.pbCardImage = new System.Windows.Forms.PictureBox();
            this.tabCollection = new System.Windows.Forms.TabPage();
            this.lvCollection = new System.Windows.Forms.ListView();
            this.chColName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chColExpansion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chColRarity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chColColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chColQuantity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlCollectionActions = new System.Windows.Forms.Panel();
            this.btnSaveCollection = new System.Windows.Forms.Button();
            this.btnLoadCollection = new System.Windows.Forms.Button();
            this.lblCollectionStats = new System.Windows.Forms.Label();
            this.lblCollectionCount = new System.Windows.Forms.Label();
            this.pnlCollectionDetail = new System.Windows.Forms.Panel();
            this.btnRemoveFromCollection = new System.Windows.Forms.Button();
            this.lblCollectionDetail = new System.Windows.Forms.Label();
            this.pbCollectionImage = new System.Windows.Forms.PictureBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCollectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCollectionAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl.SuspendLayout();
            this.tabSearch.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlPagination.SuspendLayout();
            this.pnlSearchFilters.SuspendLayout();
            this.pnlCardDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCardImage)).BeginInit();
            this.tabCollection.SuspendLayout();
            this.pnlCollectionActions.SuspendLayout();
            this.pnlCollectionDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCollectionImage)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabSearch);
            this.tabControl.Controls.Add(this.tabCollection);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 35);
            this.tabControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(2130, 1087);
            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabSearch
            // 
            this.tabSearch.Controls.Add(this.lvSearchResults);
            this.tabSearch.Controls.Add(this.pnlSearch);
            this.tabSearch.Controls.Add(this.pnlPagination);
            this.tabSearch.Controls.Add(this.pnlSearchFilters);
            this.tabSearch.Controls.Add(this.pnlCardDetail);
            this.tabSearch.Location = new System.Drawing.Point(4, 29);
            this.tabSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabSearch.Name = "tabSearch";
            this.tabSearch.Size = new System.Drawing.Size(2122, 1054);
            this.tabSearch.TabIndex = 0;
            this.tabSearch.Text = "Search Cards";
            // 
            // lvSearchResults
            // 
            this.lvSearchResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chExpansion,
            this.chRarity,
            this.chColor,
            this.chCardType});
            this.lvSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSearchResults.FullRowSelect = true;
            this.lvSearchResults.HideSelection = false;
            this.lvSearchResults.Location = new System.Drawing.Point(300, 77);
            this.lvSearchResults.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvSearchResults.MultiSelect = false;
            this.lvSearchResults.Name = "lvSearchResults";
            this.lvSearchResults.Size = new System.Drawing.Size(1447, 932);
            this.lvSearchResults.TabIndex = 3;
            this.lvSearchResults.UseCompatibleStateImageBehavior = false;
            this.lvSearchResults.View = System.Windows.Forms.View.Details;
            this.lvSearchResults.SelectedIndexChanged += new System.EventHandler(this.lvSearchResults_SelectedIndexChanged);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 200;
            // 
            // chExpansion
            // 
            this.chExpansion.Text = "Expansion";
            this.chExpansion.Width = 100;
            // 
            // chRarity
            // 
            this.chRarity.Text = "Rarity";
            this.chRarity.Width = 70;
            // 
            // chColor
            // 
            this.chColor.Text = "Color";
            this.chColor.Width = 70;
            // 
            // chCardType
            // 
            this.chCardType.Text = "Type";
            this.chCardType.Width = 80;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.lblSearchTerm);
            this.pnlSearch.Controls.Add(this.txtSearchTerm);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.cboSearchField);
            this.pnlSearch.Controls.Add(this.lblSearchField);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(300, 0);
            this.pnlSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1447, 77);
            this.pnlSearch.TabIndex = 4;
            // 
            // lblSearchTerm
            // 
            this.lblSearchTerm.AutoSize = true;
            this.lblSearchTerm.Location = new System.Drawing.Point(18, 23);
            this.lblSearchTerm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearchTerm.Name = "lblSearchTerm";
            this.lblSearchTerm.Size = new System.Drawing.Size(104, 20);
            this.lblSearchTerm.TabIndex = 0;
            this.lblSearchTerm.Text = "Search Term:";
            // 
            // txtSearchTerm
            // 
            this.txtSearchTerm.Location = new System.Drawing.Point(132, 18);
            this.txtSearchTerm.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSearchTerm.Name = "txtSearchTerm";
            this.txtSearchTerm.Size = new System.Drawing.Size(298, 26);
            this.txtSearchTerm.TabIndex = 1;
            this.txtSearchTerm.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearchTerm_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(700, 15);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(112, 35);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cboSearchField
            // 
            this.cboSearchField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSearchField.FormattingEnabled = true;
            this.cboSearchField.Items.AddRange(new object[] {
            "Name",
            "Expansion",
            "Color",
            "CardType",
            "Traits",
            "Effect",
            "Illustrator"});
            this.cboSearchField.Location = new System.Drawing.Point(510, 18);
            this.cboSearchField.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboSearchField.Name = "cboSearchField";
            this.cboSearchField.Size = new System.Drawing.Size(180, 28);
            this.cboSearchField.TabIndex = 3;
            // 
            // lblSearchField
            // 
            this.lblSearchField.AutoSize = true;
            this.lblSearchField.Location = new System.Drawing.Point(450, 23);
            this.lblSearchField.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearchField.Name = "lblSearchField";
            this.lblSearchField.Size = new System.Drawing.Size(47, 20);
            this.lblSearchField.TabIndex = 4;
            this.lblSearchField.Text = "Field:";
            // 
            // pnlPagination
            // 
            this.pnlPagination.BackColor = System.Drawing.SystemColors.Control;
            this.pnlPagination.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPagination.Controls.Add(this.lblResultCount);
            this.pnlPagination.Controls.Add(this.btnPrevPage);
            this.pnlPagination.Controls.Add(this.btnNextPage);
            this.pnlPagination.Controls.Add(this.lblPageInfo);
            this.pnlPagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPagination.Location = new System.Drawing.Point(300, 1009);
            this.pnlPagination.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlPagination.Name = "pnlPagination";
            this.pnlPagination.Size = new System.Drawing.Size(1447, 45);
            this.pnlPagination.TabIndex = 5;
            // 
            // lblResultCount
            // 
            this.lblResultCount.AutoSize = true;
            this.lblResultCount.Location = new System.Drawing.Point(15, 11);
            this.lblResultCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResultCount.Name = "lblResultCount";
            this.lblResultCount.Size = new System.Drawing.Size(178, 20);
            this.lblResultCount.TabIndex = 0;
            this.lblResultCount.Text = "Search Results: 0 cards";
            // 
            // btnPrevPage
            // 
            this.btnPrevPage.Enabled = false;
            this.btnPrevPage.Location = new System.Drawing.Point(1190, 5);
            this.btnPrevPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPrevPage.Name = "btnPrevPage";
            this.btnPrevPage.Size = new System.Drawing.Size(120, 35);
            this.btnPrevPage.TabIndex = 1;
            this.btnPrevPage.Text = "< Previous";
            this.btnPrevPage.Click += new System.EventHandler(this.btnPrevPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Enabled = false;
            this.btnNextPage.Location = new System.Drawing.Point(1318, 5);
            this.btnNextPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(120, 35);
            this.btnNextPage.TabIndex = 2;
            this.btnNextPage.Text = "Next >";
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Location = new System.Drawing.Point(1044, 12);
            this.lblPageInfo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(90, 20);
            this.lblPageInfo.TabIndex = 3;
            this.lblPageInfo.Text = "Page 0 of 0";
            // 
            // pnlSearchFilters
            // 
            this.pnlSearchFilters.AutoScroll = true;
            this.pnlSearchFilters.Controls.Add(this.lblExpansion);
            this.pnlSearchFilters.Controls.Add(this.cboExpansion);
            this.pnlSearchFilters.Controls.Add(this.lblColor);
            this.pnlSearchFilters.Controls.Add(this.cboColor);
            this.pnlSearchFilters.Controls.Add(this.lblRarity);
            this.pnlSearchFilters.Controls.Add(this.cboRarity);
            this.pnlSearchFilters.Controls.Add(this.lblCardType);
            this.pnlSearchFilters.Controls.Add(this.cboCardType);
            this.pnlSearchFilters.Controls.Add(this.lblLevel);
            this.pnlSearchFilters.Controls.Add(this.cboLevel);
            this.pnlSearchFilters.Controls.Add(this.lblCost);
            this.pnlSearchFilters.Controls.Add(this.cboCost);
            this.pnlSearchFilters.Controls.Add(this.lblTrigger);
            this.pnlSearchFilters.Controls.Add(this.cboTrigger);
            this.pnlSearchFilters.Controls.Add(this.lblSide);
            this.pnlSearchFilters.Controls.Add(this.cboSide);
            this.pnlSearchFilters.Controls.Add(this.btnApplyFilters);
            this.pnlSearchFilters.Controls.Add(this.btnClearFilters);
            this.pnlSearchFilters.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSearchFilters.Location = new System.Drawing.Point(0, 0);
            this.pnlSearchFilters.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlSearchFilters.Name = "pnlSearchFilters";
            this.pnlSearchFilters.Size = new System.Drawing.Size(300, 1054);
            this.pnlSearchFilters.TabIndex = 6;
            // 
            // lblExpansion
            // 
            this.lblExpansion.AutoSize = true;
            this.lblExpansion.Location = new System.Drawing.Point(18, 23);
            this.lblExpansion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExpansion.Name = "lblExpansion";
            this.lblExpansion.Size = new System.Drawing.Size(87, 20);
            this.lblExpansion.TabIndex = 0;
            this.lblExpansion.Text = "Expansion:";
            // 
            // cboExpansion
            // 
            this.cboExpansion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExpansion.FormattingEnabled = true;
            this.cboExpansion.Location = new System.Drawing.Point(18, 54);
            this.cboExpansion.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboExpansion.Name = "cboExpansion";
            this.cboExpansion.Size = new System.Drawing.Size(253, 28);
            this.cboExpansion.TabIndex = 1;
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(18, 100);
            this.lblColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(50, 20);
            this.lblColor.TabIndex = 2;
            this.lblColor.Text = "Color:";
            // 
            // cboColor
            // 
            this.cboColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboColor.FormattingEnabled = true;
            this.cboColor.Location = new System.Drawing.Point(18, 131);
            this.cboColor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboColor.Name = "cboColor";
            this.cboColor.Size = new System.Drawing.Size(253, 28);
            this.cboColor.TabIndex = 3;
            // 
            // lblRarity
            // 
            this.lblRarity.AutoSize = true;
            this.lblRarity.Location = new System.Drawing.Point(18, 177);
            this.lblRarity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRarity.Name = "lblRarity";
            this.lblRarity.Size = new System.Drawing.Size(54, 20);
            this.lblRarity.TabIndex = 4;
            this.lblRarity.Text = "Rarity:";
            // 
            // cboRarity
            // 
            this.cboRarity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRarity.FormattingEnabled = true;
            this.cboRarity.Location = new System.Drawing.Point(18, 208);
            this.cboRarity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboRarity.Name = "cboRarity";
            this.cboRarity.Size = new System.Drawing.Size(253, 28);
            this.cboRarity.TabIndex = 5;
            // 
            // lblCardType
            // 
            this.lblCardType.AutoSize = true;
            this.lblCardType.Location = new System.Drawing.Point(18, 254);
            this.lblCardType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCardType.Name = "lblCardType";
            this.lblCardType.Size = new System.Drawing.Size(47, 20);
            this.lblCardType.TabIndex = 6;
            this.lblCardType.Text = "Type:";
            // 
            // cboCardType
            // 
            this.cboCardType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCardType.FormattingEnabled = true;
            this.cboCardType.Location = new System.Drawing.Point(18, 285);
            this.cboCardType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboCardType.Name = "cboCardType";
            this.cboCardType.Size = new System.Drawing.Size(253, 28);
            this.cboCardType.TabIndex = 7;
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.Location = new System.Drawing.Point(18, 331);
            this.lblLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(50, 20);
            this.lblLevel.TabIndex = 8;
            this.lblLevel.Text = "Level:";
            // 
            // cboLevel
            // 
            this.cboLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLevel.FormattingEnabled = true;
            this.cboLevel.Location = new System.Drawing.Point(18, 362);
            this.cboLevel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboLevel.Name = "cboLevel";
            this.cboLevel.Size = new System.Drawing.Size(253, 28);
            this.cboLevel.TabIndex = 9;
            // 
            // lblCost
            // 
            this.lblCost.AutoSize = true;
            this.lblCost.Location = new System.Drawing.Point(18, 408);
            this.lblCost.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCost.Name = "lblCost";
            this.lblCost.Size = new System.Drawing.Size(46, 20);
            this.lblCost.TabIndex = 10;
            this.lblCost.Text = "Cost:";
            // 
            // cboCost
            // 
            this.cboCost.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCost.FormattingEnabled = true;
            this.cboCost.Location = new System.Drawing.Point(18, 438);
            this.cboCost.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboCost.Name = "cboCost";
            this.cboCost.Size = new System.Drawing.Size(253, 28);
            this.cboCost.TabIndex = 11;
            // 
            // lblTrigger
            // 
            this.lblTrigger.AutoSize = true;
            this.lblTrigger.Location = new System.Drawing.Point(18, 485);
            this.lblTrigger.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTrigger.Name = "lblTrigger";
            this.lblTrigger.Size = new System.Drawing.Size(62, 20);
            this.lblTrigger.TabIndex = 12;
            this.lblTrigger.Text = "Trigger:";
            // 
            // cboTrigger
            // 
            this.cboTrigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrigger.FormattingEnabled = true;
            this.cboTrigger.Location = new System.Drawing.Point(18, 515);
            this.cboTrigger.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboTrigger.Name = "cboTrigger";
            this.cboTrigger.Size = new System.Drawing.Size(253, 28);
            this.cboTrigger.TabIndex = 13;
            // 
            // lblSide
            // 
            this.lblSide.AutoSize = true;
            this.lblSide.Location = new System.Drawing.Point(18, 562);
            this.lblSide.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSide.Name = "lblSide";
            this.lblSide.Size = new System.Drawing.Size(45, 20);
            this.lblSide.TabIndex = 14;
            this.lblSide.Text = "Side:";
            // 
            // cboSide
            // 
            this.cboSide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSide.FormattingEnabled = true;
            this.cboSide.Location = new System.Drawing.Point(18, 592);
            this.cboSide.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboSide.Name = "cboSide";
            this.cboSide.Size = new System.Drawing.Size(253, 28);
            this.cboSide.TabIndex = 15;
            // 
            // btnApplyFilters
            // 
            this.btnApplyFilters.Location = new System.Drawing.Point(18, 646);
            this.btnApplyFilters.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnApplyFilters.Name = "btnApplyFilters";
            this.btnApplyFilters.Size = new System.Drawing.Size(128, 35);
            this.btnApplyFilters.TabIndex = 16;
            this.btnApplyFilters.Text = "Apply Filters";
            this.btnApplyFilters.UseVisualStyleBackColor = true;
            this.btnApplyFilters.Click += new System.EventHandler(this.btnApplyFilters_Click);
            // 
            // btnClearFilters
            // 
            this.btnClearFilters.Location = new System.Drawing.Point(154, 646);
            this.btnClearFilters.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new System.Drawing.Size(128, 35);
            this.btnClearFilters.TabIndex = 17;
            this.btnClearFilters.Text = "Clear Filters";
            this.btnClearFilters.UseVisualStyleBackColor = true;
            this.btnClearFilters.Click += new System.EventHandler(this.btnClearFilters_Click);
            // 
            // pnlCardDetail
            // 
            this.pnlCardDetail.AutoScroll = true;
            this.pnlCardDetail.Controls.Add(this.btnAddToCollection);
            this.pnlCardDetail.Controls.Add(this.lblCardDetail);
            this.pnlCardDetail.Controls.Add(this.pbCardImage);
            this.pnlCardDetail.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlCardDetail.Location = new System.Drawing.Point(1747, 0);
            this.pnlCardDetail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlCardDetail.Name = "pnlCardDetail";
            this.pnlCardDetail.Size = new System.Drawing.Size(375, 1054);
            this.pnlCardDetail.TabIndex = 7;
            // 
            // btnAddToCollection
            // 
            this.btnAddToCollection.Location = new System.Drawing.Point(22, 505);
            this.btnAddToCollection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddToCollection.Name = "btnAddToCollection";
            this.btnAddToCollection.Size = new System.Drawing.Size(330, 35);
            this.btnAddToCollection.TabIndex = 0;
            this.btnAddToCollection.Text = "Add to Collection";
            this.btnAddToCollection.UseVisualStyleBackColor = true;
            this.btnAddToCollection.Click += new System.EventHandler(this.btnAddToCollection_Click);
            // 
            // lblCardDetail
            // 
            this.lblCardDetail.AutoSize = true;
            this.lblCardDetail.Location = new System.Drawing.Point(18, 553);
            this.lblCardDetail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCardDetail.MaximumSize = new System.Drawing.Size(330, 0);
            this.lblCardDetail.Name = "lblCardDetail";
            this.lblCardDetail.Size = new System.Drawing.Size(100, 20);
            this.lblCardDetail.TabIndex = 1;
            this.lblCardDetail.Text = "Card Details:";
            // 
            // pbCardImage
            // 
            this.pbCardImage.Location = new System.Drawing.Point(22, 23);
            this.pbCardImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbCardImage.Name = "pbCardImage";
            this.pbCardImage.Size = new System.Drawing.Size(330, 472);
            this.pbCardImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCardImage.TabIndex = 2;
            this.pbCardImage.TabStop = false;
            // 
            // tabCollection
            // 
            this.tabCollection.Controls.Add(this.lvCollection);
            this.tabCollection.Controls.Add(this.pnlCollectionActions);
            this.tabCollection.Controls.Add(this.pnlCollectionDetail);
            this.tabCollection.Location = new System.Drawing.Point(4, 29);
            this.tabCollection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabCollection.Name = "tabCollection";
            this.tabCollection.Size = new System.Drawing.Size(1468, 793);
            this.tabCollection.TabIndex = 1;
            this.tabCollection.Text = "My Collection";
            // 
            // lvCollection
            // 
            this.lvCollection.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chColName,
            this.chColExpansion,
            this.chColRarity,
            this.chColColor,
            this.chColQuantity});
            this.lvCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvCollection.FullRowSelect = true;
            this.lvCollection.HideSelection = false;
            this.lvCollection.Location = new System.Drawing.Point(0, 77);
            this.lvCollection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvCollection.MultiSelect = false;
            this.lvCollection.Name = "lvCollection";
            this.lvCollection.Size = new System.Drawing.Size(1093, 716);
            this.lvCollection.TabIndex = 6;
            this.lvCollection.UseCompatibleStateImageBehavior = false;
            this.lvCollection.View = System.Windows.Forms.View.Details;
            this.lvCollection.SelectedIndexChanged += new System.EventHandler(this.lvCollection_SelectedIndexChanged);
            // 
            // chColName
            // 
            this.chColName.Text = "Name";
            this.chColName.Width = 200;
            // 
            // chColExpansion
            // 
            this.chColExpansion.Text = "Expansion";
            this.chColExpansion.Width = 100;
            // 
            // chColRarity
            // 
            this.chColRarity.Text = "Rarity";
            this.chColRarity.Width = 70;
            // 
            // chColColor
            // 
            this.chColColor.Text = "Color";
            this.chColColor.Width = 70;
            // 
            // chColQuantity
            // 
            this.chColQuantity.Text = "Quantity";
            // 
            // pnlCollectionActions
            // 
            this.pnlCollectionActions.Controls.Add(this.btnSaveCollection);
            this.pnlCollectionActions.Controls.Add(this.btnLoadCollection);
            this.pnlCollectionActions.Controls.Add(this.lblCollectionStats);
            this.pnlCollectionActions.Controls.Add(this.lblCollectionCount);
            this.pnlCollectionActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCollectionActions.Location = new System.Drawing.Point(0, 0);
            this.pnlCollectionActions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlCollectionActions.Name = "pnlCollectionActions";
            this.pnlCollectionActions.Size = new System.Drawing.Size(1093, 77);
            this.pnlCollectionActions.TabIndex = 7;
            // 
            // btnSaveCollection
            // 
            this.btnSaveCollection.Location = new System.Drawing.Point(18, 18);
            this.btnSaveCollection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSaveCollection.Name = "btnSaveCollection";
            this.btnSaveCollection.Size = new System.Drawing.Size(150, 35);
            this.btnSaveCollection.TabIndex = 0;
            this.btnSaveCollection.Text = "Save Collection";
            this.btnSaveCollection.UseVisualStyleBackColor = true;
            this.btnSaveCollection.Click += new System.EventHandler(this.btnSaveCollection_Click);
            // 
            // btnLoadCollection
            // 
            this.btnLoadCollection.Location = new System.Drawing.Point(177, 18);
            this.btnLoadCollection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLoadCollection.Name = "btnLoadCollection";
            this.btnLoadCollection.Size = new System.Drawing.Size(150, 35);
            this.btnLoadCollection.TabIndex = 1;
            this.btnLoadCollection.Text = "Load Collection";
            this.btnLoadCollection.UseVisualStyleBackColor = true;
            this.btnLoadCollection.Click += new System.EventHandler(this.btnLoadCollection_Click);
            // 
            // lblCollectionStats
            // 
            this.lblCollectionStats.AutoSize = true;
            this.lblCollectionStats.Location = new System.Drawing.Point(345, 26);
            this.lblCollectionStats.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCollectionStats.Name = "lblCollectionStats";
            this.lblCollectionStats.Size = new System.Drawing.Size(154, 20);
            this.lblCollectionStats.TabIndex = 2;
            this.lblCollectionStats.Text = "Cards in collection: 0";
            // 
            // lblCollectionCount
            // 
            this.lblCollectionCount.AutoSize = true;
            this.lblCollectionCount.Location = new System.Drawing.Point(675, 26);
            this.lblCollectionCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCollectionCount.Name = "lblCollectionCount";
            this.lblCollectionCount.Size = new System.Drawing.Size(138, 20);
            this.lblCollectionCount.TabIndex = 3;
            this.lblCollectionCount.Text = "Collection: 0 cards";
            // 
            // pnlCollectionDetail
            // 
            this.pnlCollectionDetail.AutoScroll = true;
            this.pnlCollectionDetail.Controls.Add(this.btnRemoveFromCollection);
            this.pnlCollectionDetail.Controls.Add(this.lblCollectionDetail);
            this.pnlCollectionDetail.Controls.Add(this.pbCollectionImage);
            this.pnlCollectionDetail.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlCollectionDetail.Location = new System.Drawing.Point(1093, 0);
            this.pnlCollectionDetail.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlCollectionDetail.Name = "pnlCollectionDetail";
            this.pnlCollectionDetail.Size = new System.Drawing.Size(375, 793);
            this.pnlCollectionDetail.TabIndex = 8;
            // 
            // btnRemoveFromCollection
            // 
            this.btnRemoveFromCollection.Location = new System.Drawing.Point(22, 677);
            this.btnRemoveFromCollection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRemoveFromCollection.Name = "btnRemoveFromCollection";
            this.btnRemoveFromCollection.Size = new System.Drawing.Size(330, 35);
            this.btnRemoveFromCollection.TabIndex = 0;
            this.btnRemoveFromCollection.Text = "Remove from Collection";
            this.btnRemoveFromCollection.UseVisualStyleBackColor = true;
            this.btnRemoveFromCollection.Click += new System.EventHandler(this.btnRemoveFromCollection_Click);
            // 
            // lblCollectionDetail
            // 
            this.lblCollectionDetail.AutoSize = true;
            this.lblCollectionDetail.Location = new System.Drawing.Point(22, 508);
            this.lblCollectionDetail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCollectionDetail.MaximumSize = new System.Drawing.Size(330, 0);
            this.lblCollectionDetail.Name = "lblCollectionDetail";
            this.lblCollectionDetail.Size = new System.Drawing.Size(100, 20);
            this.lblCollectionDetail.TabIndex = 1;
            this.lblCollectionDetail.Text = "Card Details:";
            // 
            // pbCollectionImage
            // 
            this.pbCollectionImage.Location = new System.Drawing.Point(22, 23);
            this.pbCollectionImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbCollectionImage.Name = "pbCollectionImage";
            this.pbCollectionImage.Size = new System.Drawing.Size(330, 472);
            this.pbCollectionImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCollectionImage.TabIndex = 2;
            this.pbCollectionImage.TabStop = false;
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip.Size = new System.Drawing.Size(2130, 35);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newCollectionToolStripMenuItem,
            this.openCollectionToolStripMenuItem,
            this.saveCollectionToolStripMenuItem,
            this.saveCollectionAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newCollectionToolStripMenuItem
            // 
            this.newCollectionToolStripMenuItem.Name = "newCollectionToolStripMenuItem";
            this.newCollectionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newCollectionToolStripMenuItem.Size = new System.Drawing.Size(306, 34);
            this.newCollectionToolStripMenuItem.Text = "&New Collection";
            this.newCollectionToolStripMenuItem.Click += new System.EventHandler(this.newCollectionToolStripMenuItem_Click);
            // 
            // openCollectionToolStripMenuItem
            // 
            this.openCollectionToolStripMenuItem.Name = "openCollectionToolStripMenuItem";
            this.openCollectionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openCollectionToolStripMenuItem.Size = new System.Drawing.Size(306, 34);
            this.openCollectionToolStripMenuItem.Text = "&Open Collection";
            this.openCollectionToolStripMenuItem.Click += new System.EventHandler(this.openCollectionToolStripMenuItem_Click);
            // 
            // saveCollectionToolStripMenuItem
            // 
            this.saveCollectionToolStripMenuItem.Name = "saveCollectionToolStripMenuItem";
            this.saveCollectionToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveCollectionToolStripMenuItem.Size = new System.Drawing.Size(306, 34);
            this.saveCollectionToolStripMenuItem.Text = "&Save Collection";
            this.saveCollectionToolStripMenuItem.Click += new System.EventHandler(this.saveCollectionToolStripMenuItem_Click);
            // 
            // saveCollectionAsToolStripMenuItem
            // 
            this.saveCollectionAsToolStripMenuItem.Name = "saveCollectionAsToolStripMenuItem";
            this.saveCollectionAsToolStripMenuItem.Size = new System.Drawing.Size(306, 34);
            this.saveCollectionAsToolStripMenuItem.Text = "Save Collection &As";
            this.saveCollectionAsToolStripMenuItem.Click += new System.EventHandler(this.saveCollectionAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(303, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(306, 34);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2130, 1122);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(1189, 893);
            this.Name = "MainForm";
            this.Text = "Trading Card Manager";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabSearch.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlPagination.ResumeLayout(false);
            this.pnlPagination.PerformLayout();
            this.pnlSearchFilters.ResumeLayout(false);
            this.pnlSearchFilters.PerformLayout();
            this.pnlCardDetail.ResumeLayout(false);
            this.pnlCardDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCardImage)).EndInit();
            this.tabCollection.ResumeLayout(false);
            this.pnlCollectionActions.ResumeLayout(false);
            this.pnlCollectionActions.PerformLayout();
            this.pnlCollectionDetail.ResumeLayout(false);
            this.pnlCollectionDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCollectionImage)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Check database connection
            if (!dbHelper.CheckConnection())
            {
                MessageBox.Show("Could not connect to the database. Please make sure the WSCards.db file is in the application directory.",
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Load predefined values for filters
            PopulateFilterDropdowns();

            // Load collection if exists
            RefreshCollectionView();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Auto-refresh collection view when switching to the collection tab
            if (tabControl.SelectedTab == tabCollection)
            {
                RefreshCollectionView();
                UpdateCollectionStats();
            }
        }

        private void PopulateFilterDropdowns()
        {
            // Card Types
            var cardTypeChoices = new List<string>
            {
                "",
                "Rule",
                "Character",
                "Climax",
                "Event"
            };

            // Rarity
            var rarityChoices = new List<string>
            {
                "",
                "TD ", "PR ", "SR ", "RR ", "SP ", "R ", "RRR ", "U ", "C ", "CR ", "CC ", "XR ",
                "RR ", "SSP ", "μR ", "R ", "U ", "C ", "PPR ", "SPM ", "SEC ", "GGR ", "HR ",
                "BDR ", "STR ", "DD ", "JJR ", "FBR ", "KR ", "SSP ", "OFR ", "HYR ", "RBR ",
                "PFR ", "RTR ", "TRV ", "TTR ", "N ", "HLP ", "MDR ", "DSR ", "PS ", "ATR ",
                "SCC ", "OFR ", "SPYR "
            };

            // Level
            var levelChoices = new List<string>
            {
                "",
                "0", "1", "2", "3"
            };

            // Soul
            var soulChoices = new List<string>
            {
                "",
                "0", "1", "2", "3"
            };

            // Power
            var powerChoices = new List<string>
            {
                "",
                "500", "1000", "1500", "2000", "2500", "3000", "3500", "4000", "4500", "5000",
                "5500", "6000", "6500", "7000", "7500", "8000", "8500", "9000", "9500", "10000"
            };

            // Trigger
            var triggerChoices = new List<string>
            {
                "",
                "Soul+1", "Soul+2", "Pool", "Comeback", "Return", "Draw",
                "Treasure", "Shot", "Gate", "Choice", "Standby"
            };

            // Clear all dropdowns before populating
            cboExpansion.Items.Clear();
            cboColor.Items.Clear();
            cboRarity.Items.Clear();
            cboCardType.Items.Clear();
            cboLevel.Items.Clear();
            cboCost.Items.Clear();
            cboTrigger.Items.Clear();
            cboSide.Items.Clear();

            // Add static choices
            foreach (var type in cardTypeChoices)
                cboCardType.Items.Add(type);

            foreach (var rarity in rarityChoices)
                cboRarity.Items.Add(rarity);

            foreach (var level in levelChoices)
                cboLevel.Items.Add(level);

            foreach (var cost in powerChoices) // Using power choices for cost
                cboCost.Items.Add(cost);

            foreach (var trigger in triggerChoices)
                cboTrigger.Items.Add(trigger);

            // Add the empty item first
            cboExpansion.Items.Add("");
            cboColor.Items.Add("");
            cboSide.Items.Add("");

            // Get values from database for completeness
            try
            {
                List<string> expansions = dbHelper.GetAllExpansions();
                foreach (var expansion in expansions)
                {
                    if (!cboExpansion.Items.Contains(expansion))
                        cboExpansion.Items.Add(expansion);
                }

                List<string> colors = dbHelper.GetAllColors();
                foreach (var color in colors)
                {
                    if (!cboColor.Items.Contains(color))
                        cboColor.Items.Add(color);
                }

                List<string> sides = dbHelper.GetAllSides();
                foreach (var side in sides)
                {
                    if (!cboSide.Items.Contains(side))
                        cboSide.Items.Add(side);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error populating dropdowns: {ex.Message}");
            }

            // Select empty items by default
            cboExpansion.SelectedIndex = 0;
            cboColor.SelectedIndex = 0;
            cboRarity.SelectedIndex = 0;
            cboCardType.SelectedIndex = 0;
            cboLevel.SelectedIndex = 0;
            cboCost.SelectedIndex = 0;
            cboTrigger.SelectedIndex = 0;
            cboSide.SelectedIndex = 0;
        }

        private void txtSearchTerm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(this, EventArgs.Empty);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearchTerm.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                string searchField = cboSearchField.SelectedItem.ToString();
                currentSearchResults = dbHelper.SearchCards(searchTerm, searchField);

                // Update the results count and reset page
                currentPage = 0;
                totalCardCount = currentSearchResults.Count;

                UpdateSearchResultsDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error performing search: {ex.Message}", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnApplyFilters_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Get filter values
                currentExpansionFilter = cboExpansion.SelectedItem.ToString();
                currentColorFilter = cboColor.SelectedItem.ToString();
                currentRarityFilter = cboRarity.SelectedItem.ToString();
                currentCardTypeFilter = cboCardType.SelectedItem.ToString();
                currentLevelFilter = cboLevel.SelectedItem.ToString();
                currentCostFilter = cboCost.SelectedItem.ToString();
                currentTriggerFilter = cboTrigger.SelectedItem.ToString();
                currentSideFilter = cboSide.SelectedItem.ToString();

                // Check for all character query which can be memory intensive
                bool isAllCharacterQuery = currentCardTypeFilter == "Character" &&
                                          string.IsNullOrEmpty(currentExpansionFilter) &&
                                          string.IsNullOrEmpty(currentColorFilter) &&
                                          string.IsNullOrEmpty(currentRarityFilter) &&
                                          string.IsNullOrEmpty(currentLevelFilter) &&
                                          string.IsNullOrEmpty(currentCostFilter) &&
                                          string.IsNullOrEmpty(currentTriggerFilter) &&
                                          string.IsNullOrEmpty(currentSideFilter);

                if (isAllCharacterQuery)
                {
                    DialogResult result = MessageBox.Show(
                        "Querying all character cards may take a long time and use significant memory. " +
                        "Consider adding additional filters.\n\nContinue anyway?",
                        "Large Query Warning",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.No)
                    {
                        Cursor = Cursors.Default;
                        return;
                    }
                }

                // First get the total count
                totalCardCount = dbHelper.CountCardsByFilter(
                    string.IsNullOrEmpty(currentExpansionFilter) ? null : currentExpansionFilter,
                    string.IsNullOrEmpty(currentColorFilter) ? null : currentColorFilter,
                    string.IsNullOrEmpty(currentRarityFilter) ? null : currentRarityFilter,
                    string.IsNullOrEmpty(currentCardTypeFilter) ? null : currentCardTypeFilter,
                    string.IsNullOrEmpty(currentLevelFilter) ? null : currentLevelFilter,
                    string.IsNullOrEmpty(currentCostFilter) ? null : currentCostFilter,
                    string.IsNullOrEmpty(currentTriggerFilter) ? null : currentTriggerFilter,
                    string.IsNullOrEmpty(currentSideFilter) ? null : currentSideFilter);

                // Set appropriate page size for large result sets
                if (totalCardCount > 1000)
                {
                    pageSize = 50; // Use smaller page size for large results
                }
                else
                {
                    pageSize = 100; // Normal page size
                }

                // Reset to first page
                currentPage = 0;

                // Load the first page of results
                LoadCurrentPage();
            }
            catch (OutOfMemoryException)
            {
                MessageBox.Show(
                    "The query resulted in too many cards and ran out of memory. Please use more specific filters.",
                    "Out of Memory Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Clear results
                currentSearchResults = new List<Card>();
                UpdateSearchResultsDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"An error occurred while filtering cards: {ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void LoadCurrentPage()
        {
            try
            {
                // Determine if we need to load images (not for large result sets)
                bool loadImages = totalCardCount <= 1000;

                // Get the current page of results
                currentSearchResults = dbHelper.GetCardsByFilterPaged(
                    currentPage,
                    pageSize,
                    string.IsNullOrEmpty(currentExpansionFilter) ? null : currentExpansionFilter,
                    string.IsNullOrEmpty(currentColorFilter) ? null : currentColorFilter,
                    string.IsNullOrEmpty(currentRarityFilter) ? null : currentRarityFilter,
                    string.IsNullOrEmpty(currentCardTypeFilter) ? null : currentCardTypeFilter,
                    string.IsNullOrEmpty(currentLevelFilter) ? null : currentLevelFilter,
                    string.IsNullOrEmpty(currentCostFilter) ? null : currentCostFilter,
                    string.IsNullOrEmpty(currentTriggerFilter) ? null : currentTriggerFilter,
                    string.IsNullOrEmpty(currentSideFilter) ? null : currentSideFilter,
                    loadImages);

                // Update display
                UpdateSearchResultsDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading page: {ex.Message}", "Page Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateSearchResultsDisplay()
        {
            lvSearchResults.Items.Clear();

            // Add each card to the list view
            foreach (var card in currentSearchResults)
            {
                ListViewItem item = new ListViewItem(card.Name);
                item.SubItems.Add(card.Expansion);
                item.SubItems.Add(card.Rarity);
                item.SubItems.Add(card.Color);
                item.SubItems.Add(card.CardType);
                item.Tag = card;

                lvSearchResults.Items.Add(item);
            }

            // Update count and pagination info
            int totalPages = (int)Math.Ceiling((double)totalCardCount / pageSize);
            lblResultCount.Text = $"Search Results: {totalCardCount} cards";
            lblPageInfo.Text = totalPages > 0 ? $"Page {currentPage + 1} of {totalPages}" : "No results";

            // Enable/disable pagination buttons
            btnPrevPage.Enabled = currentPage > 0;
            btnNextPage.Enabled = (currentPage + 1) < totalPages;

            // Update window title
            this.Text = $"Trading Card Manager - {totalCardCount} cards found" +
                        (totalPages > 1 ? $" (Page {currentPage + 1}/{totalPages})" : "");

            // Clear card details
            lblCardDetail.Text = "Card Details:";
            pbCardImage.Image = null;
        }

        private void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (currentPage > 0)
            {
                currentPage--;
                LoadCurrentPage();
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            int totalPages = (int)Math.Ceiling((double)totalCardCount / pageSize);
            if (currentPage + 1 < totalPages)
            {
                currentPage++;
                LoadCurrentPage();
            }
        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            // Reset filters
            cboExpansion.SelectedIndex = 0;
            cboColor.SelectedIndex = 0;
            cboRarity.SelectedIndex = 0;
            cboCardType.SelectedIndex = 0;
            cboLevel.SelectedIndex = 0;
            cboCost.SelectedIndex = 0;
            cboTrigger.SelectedIndex = 0;
            cboSide.SelectedIndex = 0;

            // Reset filter variables
            currentExpansionFilter = "";
            currentColorFilter = "";
            currentRarityFilter = "";
            currentCardTypeFilter = "";
            currentLevelFilter = "";
            currentCostFilter = "";
            currentTriggerFilter = "";
            currentSideFilter = "";

            // Clear results
            currentSearchResults.Clear();
            totalCardCount = 0;
            currentPage = 0;
            UpdateSearchResultsDisplay();
        }

        private void lvSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSearchResults.SelectedItems.Count > 0)
            {
                Card selectedCard = (Card)lvSearchResults.SelectedItems[0].Tag;

                // Load image data on demand if needed
                if (selectedCard.ImageData == null && !string.IsNullOrEmpty(selectedCard.CardId))
                {
                    Cursor = Cursors.WaitCursor;
                    try
                    {
                        selectedCard.ImageData = dbHelper.GetCardImageById(selectedCard.CardId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading card image: {ex.Message}");
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }
                }

                DisplayCardDetails(selectedCard, lblCardDetail, pbCardImage);
            }
        }

        private void DisplayCardDetails(Card card, Label detailLabel, PictureBox pictureBox)
        {
            if (card != null)
            {
                // Display image if available
                if (card.ImageData != null && card.ImageData.Length > 0)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(card.ImageData))
                        {
                            pictureBox.Image = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error loading card image: {ex.Message}");
                        pictureBox.Image = null;
                    }
                }
                else
                {
                    pictureBox.Image = null;
                }

                // Build details text
                string details = $"Name: {card.Name}\r\n" +
                                $"Expansion: {card.Expansion}\r\n" +
                                $"Rarity: {card.Rarity}\r\n" +
                                $"Color: {card.Color}\r\n" +
                                $"Type: {card.CardType}\r\n";

                if (!string.IsNullOrEmpty(card.Level))
                    details += $"Level: {card.Level}\r\n";

                if (!string.IsNullOrEmpty(card.Power))
                    details += $"Power: {card.Power}\r\n";

                if (!string.IsNullOrEmpty(card.Soul))
                    details += $"Soul: {card.Soul}\r\n";

                if (!string.IsNullOrEmpty(card.Cost))
                    details += $"Cost: {card.Cost}\r\n";

                if (!string.IsNullOrEmpty(card.Trigger))
                    details += $"Trigger: {card.Trigger}\r\n";

                if (!string.IsNullOrEmpty(card.Traits))
                    details += $"Traits: {card.Traits}\r\n";

                if (!string.IsNullOrEmpty(card.Effect))
                {
                    details += $"\r\nEffect:\r\n{card.Effect}\r\n";
                }

                if (!string.IsNullOrEmpty(card.Flavor))
                {
                    details += $"\r\nFlavor:\r\n{card.Flavor}\r\n";
                }

                if (!string.IsNullOrEmpty(card.Illustrator))
                    details += $"\r\nIllustrator: {card.Illustrator}\r\n";

                if (!string.IsNullOrEmpty(card.Side))
                    details += $"Side: {card.Side}";

                detailLabel.Text = details;
            }
        }

        private void btnAddToCollection_Click(object sender, EventArgs e)
        {
            if (lvSearchResults.SelectedItems.Count > 0)
            {
                Card selectedCard = (Card)lvSearchResults.SelectedItems[0].Tag;

                // Ask for quantity
                using (NumericInputDialog dialog = new NumericInputDialog("Add to Collection", "Quantity:"))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        int quantity = dialog.Value;
                        if (quantity > 0)
                        {
                            if (collectionHelper.AddCardToCollection(currentCollection, selectedCard.CardId, quantity))
                            {
                                MessageBox.Show($"Added {quantity} copies of '{selectedCard.Name}' to your collection.",
                                    "Card Added", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Auto-save collection
                                collectionHelper.SaveCollection(currentCollection, currentCollectionPath);

                                // If we're on the collection tab, refresh it
                                if (tabControl.SelectedTab == tabCollection)
                                {
                                    RefreshCollectionView();
                                }

                                UpdateCollectionStats();
                            }
                        }
                    }
                }
            }
        }

        private void btnRemoveFromCollection_Click(object sender, EventArgs e)
        {
            if (lvCollection.SelectedItems.Count > 0)
            {
                Card selectedCard = (Card)lvCollection.SelectedItems[0].Tag;
                int currentQuantity = selectedCard.QuantityInCollection;

                // Ask for quantity to remove
                using (NumericInputDialog dialog = new NumericInputDialog("Remove from Collection", "Quantity:", 1, currentQuantity))
                {
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        int quantity = dialog.Value;
                        if (quantity > 0)
                        {
                            if (collectionHelper.RemoveCardFromCollection(currentCollection, selectedCard.CardId, quantity))
                            {
                                MessageBox.Show($"Removed {quantity} copies of '{selectedCard.Name}' from your collection.",
                                    "Card Removed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Auto-save collection
                                collectionHelper.SaveCollection(currentCollection, currentCollectionPath);

                                // Refresh the collection view
                                RefreshCollectionView();
                                UpdateCollectionStats();
                            }
                        }
                    }
                }
            }
        }

        private void RefreshCollectionView()
        {
            // Save current selection if any
            string selectedCardId = null;
            if (lvCollection.SelectedItems.Count > 0)
            {
                Card selectedCard = (Card)lvCollection.SelectedItems[0].Tag;
                selectedCardId = selectedCard.CardId;
            }

            lvCollection.Items.Clear();

            List<Card> collectionCards = collectionHelper.GetCardsInCollection(currentCollection);

            foreach (var card in collectionCards)
            {
                ListViewItem item = new ListViewItem(card.Name);
                item.SubItems.Add(card.Expansion);
                item.SubItems.Add(card.Rarity);
                item.SubItems.Add(card.Color);
                item.SubItems.Add(card.QuantityInCollection.ToString());
                item.Tag = card;

                lvCollection.Items.Add(item);

                // Restore selection if this was the previously selected card
                if (selectedCardId != null && card.CardId == selectedCardId)
                {
                    item.Selected = true;
                    item.EnsureVisible();
                }
            }

            if (lvCollection.SelectedItems.Count == 0)
            {
                lblCollectionDetail.Text = "Card Details:";
                pbCollectionImage.Image = null;
            }

            // Update collection count
            UpdateCollectionStats();
        }

        private void UpdateCollectionStats()
        {
            int totalCards = currentCollection.Cards.Sum(c => c.Quantity);
            int uniqueCards = currentCollection.Cards.Count;

            lblCollectionStats.Text = $"Cards in collection: {totalCards} ({uniqueCards} unique)";
            lblCollectionCount.Text = $"Collection: {uniqueCards} unique cards ({totalCards} total)";
        }

        private void lvCollection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCollection.SelectedItems.Count > 0)
            {
                Card selectedCard = (Card)lvCollection.SelectedItems[0].Tag;
                DisplayCardDetails(selectedCard, lblCollectionDetail, pbCollectionImage);
            }
        }

        private void btnSaveCollection_Click(object sender, EventArgs e)
        {
            SaveCollection();
        }

        private void SaveCollection()
        {
            if (collectionHelper.SaveCollection(currentCollection, currentCollectionPath))
            {
                MessageBox.Show($"Collection saved to {currentCollectionPath}",
                    "Collection Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to save collection. Please try again.",
                    "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadCollection_Click(object sender, EventArgs e)
        {
            LoadCollection();
        }

        private void LoadCollection()
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "JSON Files (*.json)|*.json";
                dialog.Title = "Open Collection";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    currentCollectionPath = dialog.FileName;
                    currentCollection = collectionHelper.LoadCollection(currentCollectionPath);
                    RefreshCollectionView();
                    UpdateCollectionStats();
                }
            }
        }

        private void newCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentCollection.Cards.Count > 0)
            {
                DialogResult result = MessageBox.Show("Do you want to save your current collection first?",
                    "New Collection", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SaveFileDialog dialog = new SaveFileDialog())
                    {
                        dialog.Filter = "JSON Files (*.json)|*.json";
                        dialog.Title = "Save Current Collection As";
                        dialog.FileName = Path.GetFileName(currentCollectionPath);

                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            string savePath = dialog.FileName;
                            if (collectionHelper.SaveCollection(currentCollection, savePath))
                            {
                                MessageBox.Show($"Collection saved to {savePath}",
                                    "Collection Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Failed to save collection. Please try again.",
                                    "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            // User cancelled the save dialog
                            DialogResult continueResult = MessageBox.Show("Create new collection without saving?",
                                "New Collection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (continueResult == DialogResult.No)
                            {
                                return;
                            }
                        }
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            // Create new collection
            currentCollection = new Collection();

            // Generate a unique filename for the new collection
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string baseFilename = "CardCollection";
            string extension = ".json";
            string newFilename = baseFilename + extension;
            int counter = 1;

            while (File.Exists(Path.Combine(documentsPath, newFilename)))
            {
                newFilename = $"{baseFilename}_{counter}{extension}";
                counter++;
            }

            currentCollectionPath = Path.Combine(documentsPath, newFilename);

            RefreshCollectionView();
            UpdateCollectionStats();

            // Show the collection tab
            tabControl.SelectedTab = tabCollection;

            MessageBox.Show($"Created new collection with filename: {newFilename}",
                "New Collection", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void openCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadCollection();
        }

        private void saveCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCollection();
        }

        private void saveCollectionAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "JSON Files (*.json)|*.json";
                dialog.Title = "Save Collection As";
                dialog.FileName = Path.GetFileName(currentCollectionPath);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    currentCollectionPath = dialog.FileName;
                    SaveCollection();
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (currentCollection.Cards.Count > 0)
            {
                DialogResult result = MessageBox.Show("Do you want to save your collection before exiting?",
                    "Exit Application", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveCollection();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }

            base.OnFormClosing(e);
        }
    }
}
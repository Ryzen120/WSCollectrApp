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

        private Panel pnlSearchFilters;
        private Label lblExpansion;
        private ComboBox cboExpansion;
        private Label lblColor;
        private ComboBox cboColor;
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
            // TabControl
            this.tabControl = new TabControl();
            this.tabSearch = new TabPage();
            this.tabCollection = new TabPage();

            // Search tab controls
            this.pnlSearch = new Panel();
            this.lblSearchTerm = new Label();
            this.txtSearchTerm = new TextBox();
            this.btnSearch = new Button();
            this.cboSearchField = new ComboBox();
            this.lblSearchField = new Label();

            this.lvSearchResults = new ListView();
            this.chName = new ColumnHeader();
            this.chExpansion = new ColumnHeader();
            this.chRarity = new ColumnHeader();
            this.chColor = new ColumnHeader();
            this.chCardType = new ColumnHeader();

            this.pnlSearchFilters = new Panel();
            this.lblExpansion = new Label();
            this.cboExpansion = new ComboBox();
            this.lblColor = new Label();
            this.cboColor = new ComboBox();
            this.btnApplyFilters = new Button();
            this.btnClearFilters = new Button();

            this.pnlCardDetail = new Panel();
            this.btnAddToCollection = new Button();
            this.lblCardDetail = new Label();
            this.pbCardImage = new PictureBox();

            // Collection tab controls
            this.lvCollection = new ListView();
            this.chColName = new ColumnHeader();
            this.chColExpansion = new ColumnHeader();
            this.chColRarity = new ColumnHeader();
            this.chColColor = new ColumnHeader();
            this.chColQuantity = new ColumnHeader();

            this.pnlCollectionActions = new Panel();
            this.btnSaveCollection = new Button();
            this.btnLoadCollection = new Button();
            this.lblCollectionStats = new Label();

            this.pnlCollectionDetail = new Panel();
            this.btnRemoveFromCollection = new Button();
            this.lblCollectionDetail = new Label();
            this.pbCollectionImage = new PictureBox();

            // Menu
            this.menuStrip = new MenuStrip();
            this.fileToolStripMenuItem = new ToolStripMenuItem();
            this.newCollectionToolStripMenuItem = new ToolStripMenuItem();
            this.openCollectionToolStripMenuItem = new ToolStripMenuItem();
            this.saveCollectionToolStripMenuItem = new ToolStripMenuItem();
            this.saveCollectionAsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.exitToolStripMenuItem = new ToolStripMenuItem();

            ((System.ComponentModel.ISupportInitialize)(this.pbCardImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCollectionImage)).BeginInit();
            this.SuspendLayout();

            // TabControl
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Controls.Add(this.tabSearch);
            this.tabControl.Controls.Add(this.tabCollection);
            this.tabControl.Location = new Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(984, 537);
            this.tabControl.TabIndex = 0;

            // TabPages
            this.tabSearch.Text = "Search Cards";
            this.tabCollection.Text = "My Collection";

            // Search Tab layout
            this.tabSearch.Controls.Add(this.lvSearchResults);
            this.tabSearch.Controls.Add(this.pnlSearch);
            this.tabSearch.Controls.Add(this.pnlSearchFilters);
            this.tabSearch.Controls.Add(this.pnlCardDetail);

            // Collection Tab layout
            this.tabCollection.Controls.Add(this.lvCollection);
            this.tabCollection.Controls.Add(this.pnlCollectionActions);
            this.tabCollection.Controls.Add(this.pnlCollectionDetail);

            // Search panel
            this.pnlSearch.Dock = DockStyle.Top;
            this.pnlSearch.Height = 50;
            this.pnlSearch.Controls.Add(this.lblSearchTerm);
            this.pnlSearch.Controls.Add(this.txtSearchTerm);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Controls.Add(this.cboSearchField);
            this.pnlSearch.Controls.Add(this.lblSearchField);

            // Search Term Label
            this.lblSearchTerm.AutoSize = true;
            this.lblSearchTerm.Location = new Point(12, 15);
            this.lblSearchTerm.Name = "lblSearchTerm";
            this.lblSearchTerm.Size = new Size(70, 13);
            this.lblSearchTerm.Text = "Search Term:";

            // Search Term TextBox
            this.txtSearchTerm.Location = new Point(88, 12);
            this.txtSearchTerm.Name = "txtSearchTerm";
            this.txtSearchTerm.Size = new Size(200, 20);

            // Search Field Label
            this.lblSearchField.AutoSize = true;
            this.lblSearchField.Location = new Point(300, 15);
            this.lblSearchField.Name = "lblSearchField";
            this.lblSearchField.Size = new Size(34, 13);
            this.lblSearchField.Text = "Field:";

            // Search Field ComboBox
            this.cboSearchField.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboSearchField.FormattingEnabled = true;
            this.cboSearchField.Location = new Point(340, 12);
            this.cboSearchField.Name = "cboSearchField";
            this.cboSearchField.Size = new Size(121, 21);
            this.cboSearchField.Items.AddRange(new object[] { "Name", "Expansion", "Color", "CardType", "Traits", "Effect" });
            this.cboSearchField.SelectedIndex = 0;

            // Search Button
            this.btnSearch.Location = new Point(467, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new Size(75, 23);
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new EventHandler(this.btnSearch_Click);

            // Search Filters Panel
            this.pnlSearchFilters.Dock = DockStyle.Left;
            this.pnlSearchFilters.Width = 200;
            this.pnlSearchFilters.Controls.Add(this.lblExpansion);
            this.pnlSearchFilters.Controls.Add(this.cboExpansion);
            this.pnlSearchFilters.Controls.Add(this.lblColor);
            this.pnlSearchFilters.Controls.Add(this.cboColor);
            this.pnlSearchFilters.Controls.Add(this.btnApplyFilters);
            this.pnlSearchFilters.Controls.Add(this.btnClearFilters);

            // Expansion Label
            this.lblExpansion.AutoSize = true;
            this.lblExpansion.Location = new Point(12, 15);
            this.lblExpansion.Name = "lblExpansion";
            this.lblExpansion.Size = new Size(59, 13);
            this.lblExpansion.Text = "Expansion:";

            // Expansion ComboBox
            this.cboExpansion.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboExpansion.FormattingEnabled = true;
            this.cboExpansion.Location = new Point(12, 35);
            this.cboExpansion.Name = "cboExpansion";
            this.cboExpansion.Size = new Size(170, 21);

            // Color Label
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new Point(12, 65);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new Size(34, 13);
            this.lblColor.Text = "Color:";

            // Color ComboBox
            this.cboColor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboColor.FormattingEnabled = true;
            this.cboColor.Location = new Point(12, 85);
            this.cboColor.Name = "cboColor";
            this.cboColor.Size = new Size(170, 21);

            // Apply Filters Button
            this.btnApplyFilters.Location = new Point(12, 120);
            this.btnApplyFilters.Name = "btnApplyFilters";
            this.btnApplyFilters.Size = new Size(85, 23);
            this.btnApplyFilters.Text = "Apply Filters";
            this.btnApplyFilters.UseVisualStyleBackColor = true;
            this.btnApplyFilters.Click += new EventHandler(this.btnApplyFilters_Click);

            // Clear Filters Button
            this.btnClearFilters.Location = new Point(103, 120);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new Size(85, 23);
            this.btnClearFilters.Text = "Clear Filters";
            this.btnClearFilters.UseVisualStyleBackColor = true;
            this.btnClearFilters.Click += new EventHandler(this.btnClearFilters_Click);

            // Search Results ListView
            this.lvSearchResults.Columns.AddRange(new ColumnHeader[] {
                this.chName,
                this.chExpansion,
                this.chRarity,
                this.chColor,
                this.chCardType});
            this.lvSearchResults.Dock = DockStyle.Fill;
            this.lvSearchResults.FullRowSelect = true;
            this.lvSearchResults.HideSelection = false;
            this.lvSearchResults.Location = new Point(200, 50);
            this.lvSearchResults.MultiSelect = false;
            this.lvSearchResults.Name = "lvSearchResults";
            this.lvSearchResults.Size = new Size(534, 487);
            this.lvSearchResults.TabIndex = 3;
            this.lvSearchResults.UseCompatibleStateImageBehavior = false;
            this.lvSearchResults.View = View.Details;
            this.lvSearchResults.SelectedIndexChanged += new EventHandler(this.lvSearchResults_SelectedIndexChanged);

            // Card Detail Panel
            this.pnlCardDetail.Dock = DockStyle.Right;
            this.pnlCardDetail.Width = 250;
            this.pnlCardDetail.Controls.Add(this.btnAddToCollection);
            this.pnlCardDetail.Controls.Add(this.lblCardDetail);
            this.pnlCardDetail.Controls.Add(this.pbCardImage);

            // Card Image PictureBox
            this.pbCardImage.Location = new Point(15, 15);
            this.pbCardImage.Name = "pbCardImage";
            this.pbCardImage.Size = new Size(220, 307);
            this.pbCardImage.SizeMode = PictureBoxSizeMode.Zoom;

            // Card Detail Label
            this.lblCardDetail.AutoSize = true;
            this.lblCardDetail.Location = new Point(15, 330);
            this.lblCardDetail.Name = "lblCardDetail";
            this.lblCardDetail.Size = new Size(68, 13);
            this.lblCardDetail.Text = "Card Details:";

            // Add to Collection Button
            this.btnAddToCollection.Location = new Point(15, 440);
            this.btnAddToCollection.Name = "btnAddToCollection";
            this.btnAddToCollection.Size = new Size(220, 23);
            this.btnAddToCollection.Text = "Add to Collection";
            this.btnAddToCollection.UseVisualStyleBackColor = true;
            this.btnAddToCollection.Click += new EventHandler(this.btnAddToCollection_Click);

            // Collection ListView
            this.lvCollection.Columns.AddRange(new ColumnHeader[] {
                this.chColName,
                this.chColExpansion,
                this.chColRarity,
                this.chColColor,
                this.chColQuantity});
            this.lvCollection.Dock = DockStyle.Fill;
            this.lvCollection.FullRowSelect = true;
            this.lvCollection.HideSelection = false;
            this.lvCollection.Location = new Point(0, 50);
            this.lvCollection.MultiSelect = false;
            this.lvCollection.Name = "lvCollection";
            this.lvCollection.Size = new Size(734, 487);
            this.lvCollection.TabIndex = 6;
            this.lvCollection.UseCompatibleStateImageBehavior = false;
            this.lvCollection.View = View.Details;
            this.lvCollection.SelectedIndexChanged += new EventHandler(this.lvCollection_SelectedIndexChanged);

            // Collection Actions Panel
            this.pnlCollectionActions.Dock = DockStyle.Top;
            this.pnlCollectionActions.Height = 50;
            this.pnlCollectionActions.Controls.Add(this.btnSaveCollection);
            this.pnlCollectionActions.Controls.Add(this.btnLoadCollection);
            this.pnlCollectionActions.Controls.Add(this.lblCollectionStats);

            // Save Collection Button
            this.btnSaveCollection.Location = new Point(12, 12);
            this.btnSaveCollection.Name = "btnSaveCollection";
            this.btnSaveCollection.Size = new Size(100, 23);
            this.btnSaveCollection.Text = "Save Collection";
            this.btnSaveCollection.UseVisualStyleBackColor = true;
            this.btnSaveCollection.Click += new EventHandler(this.btnSaveCollection_Click);

            // Load Collection Button
            this.btnLoadCollection.Location = new Point(118, 12);
            this.btnLoadCollection.Name = "btnLoadCollection";
            this.btnLoadCollection.Size = new Size(100, 23);
            this.btnLoadCollection.Text = "Load Collection";
            this.btnLoadCollection.UseVisualStyleBackColor = true;
            this.btnLoadCollection.Click += new EventHandler(this.btnLoadCollection_Click);

            // Collection Stats Label
            this.lblCollectionStats.AutoSize = true;
            this.lblCollectionStats.Location = new Point(300, 17);
            this.lblCollectionStats.Name = "lblCollectionStats";
            this.lblCollectionStats.Size = new Size(121, 13);
            this.lblCollectionStats.Text = "Cards in collection: 0";

            // Collection Detail Panel
            this.pnlCollectionDetail.Dock = DockStyle.Right;
            this.pnlCollectionDetail.Width = 250;
            this.pnlCollectionDetail.Controls.Add(this.btnRemoveFromCollection);
            this.pnlCollectionDetail.Controls.Add(this.lblCollectionDetail);
            this.pnlCollectionDetail.Controls.Add(this.pbCollectionImage);

            // Collection Image PictureBox
            this.pbCollectionImage.Location = new Point(15, 15);
            this.pbCollectionImage.Name = "pbCollectionImage";
            this.pbCollectionImage.Size = new Size(220, 307);
            this.pbCollectionImage.SizeMode = PictureBoxSizeMode.Zoom;

            // Collection Detail Label
            this.lblCollectionDetail.AutoSize = true;
            this.lblCollectionDetail.Location = new Point(15, 330);
            this.lblCollectionDetail.Name = "lblCollectionDetail";
            this.lblCollectionDetail.Size = new Size(68, 13);
            this.lblCollectionDetail.Text = "Card Details:";

            // Remove from Collection Button
            this.btnRemoveFromCollection.Location = new Point(15, 440);
            this.btnRemoveFromCollection.Name = "btnRemoveFromCollection";
            this.btnRemoveFromCollection.Size = new Size(220, 23);
            this.btnRemoveFromCollection.Text = "Remove from Collection";
            this.btnRemoveFromCollection.UseVisualStyleBackColor = true;
            this.btnRemoveFromCollection.Click += new EventHandler(this.btnRemoveFromCollection_Click);

            // Menu Strip
            this.menuStrip.Items.AddRange(new ToolStripItem[] {
                this.fileToolStripMenuItem});
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(984, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";

            // File Menu
            this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] {
                this.newCollectionToolStripMenuItem,
                this.openCollectionToolStripMenuItem,
                this.saveCollectionToolStripMenuItem,
                this.saveCollectionAsToolStripMenuItem,
                this.toolStripSeparator1,
                this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";

            // New Collection Menu Item
            this.newCollectionToolStripMenuItem.Name = "newCollectionToolStripMenuItem";
            this.newCollectionToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.N)));
            this.newCollectionToolStripMenuItem.Size = new Size(186, 22);
            this.newCollectionToolStripMenuItem.Text = "&New Collection";
            this.newCollectionToolStripMenuItem.Click += new EventHandler(this.newCollectionToolStripMenuItem_Click);

            // Open Collection Menu Item
            this.openCollectionToolStripMenuItem.Name = "openCollectionToolStripMenuItem";
            this.openCollectionToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.O)));
            this.openCollectionToolStripMenuItem.Size = new Size(186, 22);
            this.openCollectionToolStripMenuItem.Text = "&Open Collection";
            this.openCollectionToolStripMenuItem.Click += new EventHandler(this.openCollectionToolStripMenuItem_Click);

            // Save Collection Menu Item
            this.saveCollectionToolStripMenuItem.Name = "saveCollectionToolStripMenuItem";
            this.saveCollectionToolStripMenuItem.ShortcutKeys = ((Keys)((Keys.Control | Keys.S)));
            this.saveCollectionToolStripMenuItem.Size = new Size(186, 22);
            this.saveCollectionToolStripMenuItem.Text = "&Save Collection";
            this.saveCollectionToolStripMenuItem.Click += new EventHandler(this.saveCollectionToolStripMenuItem_Click);

            // Save Collection As Menu Item
            this.saveCollectionAsToolStripMenuItem.Name = "saveCollectionAsToolStripMenuItem";
            this.saveCollectionAsToolStripMenuItem.Size = new Size(186, 22);
            this.saveCollectionAsToolStripMenuItem.Text = "Save Collection &As";
            this.saveCollectionAsToolStripMenuItem.Click += new EventHandler(this.saveCollectionAsToolStripMenuItem_Click);

            // Separator
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new Size(183, 6);

            // Exit Menu Item
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new Size(186, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new EventHandler(this.exitToolStripMenuItem_Click);

            // MainForm
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(984, 561);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new Size(800, 600);
            this.Name = "MainForm";
            this.Text = "Trading Card Manager";
            this.Load += new EventHandler(this.MainForm_Load);

            // Name ColumnHeader
            this.chName.Text = "Name";
            this.chName.Width = 200;

            // Expansion ColumnHeader
            this.chExpansion.Text = "Expansion";
            this.chExpansion.Width = 100;

            // Rarity ColumnHeader
            this.chRarity.Text = "Rarity";
            this.chRarity.Width = 70;

            // Color ColumnHeader
            this.chColor.Text = "Color";
            this.chColor.Width = 70;

            // CardType ColumnHeader
            this.chCardType.Text = "Type";
            this.chCardType.Width = 80;

            // Collection Name ColumnHeader
            this.chColName.Text = "Name";
            this.chColName.Width = 200;

            // Collection Expansion ColumnHeader
            this.chColExpansion.Text = "Expansion";
            this.chColExpansion.Width = 100;

            // Collection Rarity ColumnHeader
            this.chColRarity.Text = "Rarity";
            this.chColRarity.Width = 70;

            // Collection Color ColumnHeader
            this.chColColor.Text = "Color";
            this.chColColor.Width = 70;

            // Collection Quantity ColumnHeader
            this.chColQuantity.Text = "Quantity";
            this.chColQuantity.Width = 60;

            ((System.ComponentModel.ISupportInitialize)(this.pbCardImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCollectionImage)).EndInit();
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

            // Load expansions and colors for filters
            PopulateFilterDropdowns();

            // Load collection if exists
            RefreshCollectionView();
        }

        private void PopulateFilterDropdowns()
        {
            // Add empty item first
            cboExpansion.Items.Add("");
            cboColor.Items.Add("");

            // Load expansions
            List<string> expansions = dbHelper.GetAllExpansions();
            foreach (var expansion in expansions)
            {
                cboExpansion.Items.Add(expansion);
            }

            // Load colors
            List<string> colors = dbHelper.GetAllColors();
            foreach (var color in colors)
            {
                cboColor.Items.Add(color);
            }

            // Select empty items by default
            cboExpansion.SelectedIndex = 0;
            cboColor.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearchTerm.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string searchField = cboSearchField.SelectedItem.ToString();
            currentSearchResults = dbHelper.SearchCards(searchTerm, searchField);
            DisplaySearchResults();
        }

        private void btnApplyFilters_Click(object sender, EventArgs e)
        {
            string expansion = cboExpansion.SelectedItem.ToString();
            string color = cboColor.SelectedItem.ToString();

            currentSearchResults = dbHelper.GetCardsByFilter(
                string.IsNullOrEmpty(expansion) ? null : expansion,
                string.IsNullOrEmpty(color) ? null : color);

            DisplaySearchResults();
        }

        private void btnClearFilters_Click(object sender, EventArgs e)
        {
            cboExpansion.SelectedIndex = 0;
            cboColor.SelectedIndex = 0;

            // Clear results too
            currentSearchResults.Clear();
            DisplaySearchResults();
        }

        private void DisplaySearchResults()
        {
            lvSearchResults.Items.Clear();

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

            lblCardDetail.Text = "Card Details:";
            pbCardImage.Image = null;
        }

        private void lvSearchResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSearchResults.SelectedItems.Count > 0)
            {
                Card selectedCard = (Card)lvSearchResults.SelectedItems[0].Tag;
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
                    using (MemoryStream ms = new MemoryStream(card.ImageData))
                    {
                        pictureBox.Image = Image.FromStream(ms);
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
                                $"Type: {card.CardType}\r\n" +
                                $"Level: {card.Level}\r\n" +
                                $"Power: {card.Power}\r\n" +
                                $"Soul: {card.Soul}\r\n" +
                                $"Cost: {card.Cost}\r\n" +
                                $"Trigger: {card.Trigger}\r\n" +
                                $"Traits: {card.Traits}\r\n";

                if (!string.IsNullOrEmpty(card.Effect))
                {
                    details += $"\r\nEffect:\r\n{card.Effect}\r\n";
                }

                if (!string.IsNullOrEmpty(card.Flavor))
                {
                    details += $"\r\nFlavor:\r\n{card.Flavor}\r\n";
                }

                details += $"\r\nIllustrator: {card.Illustrator}";

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
            }

            lblCollectionDetail.Text = "Card Details:";
            pbCollectionImage.Image = null;
        }

        private void UpdateCollectionStats()
        {
            int totalCards = currentCollection.Cards.Sum(c => c.Quantity);
            int uniqueCards = currentCollection.Cards.Count;

            lblCollectionStats.Text = $"Cards in collection: {totalCards} ({uniqueCards} unique)";
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
                    SaveCollection();
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }

            currentCollection = new Collection();
            RefreshCollectionView();
            UpdateCollectionStats();
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
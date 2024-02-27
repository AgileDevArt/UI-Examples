namespace Wpf
{
    partial class EmployeeView
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            textId = new System.Windows.Forms.TextBox();
            textName = new System.Windows.Forms.TextBox();
            buttonRedo = new System.Windows.Forms.Button();
            buttonUndo = new System.Windows.Forms.Button();
            labelId = new System.Windows.Forms.Label();
            labelName = new System.Windows.Forms.Label();
            dataGrid = new System.Windows.Forms.DataGridView();
            buttonAbout = new System.Windows.Forms.Button();
            employeeBindingSource = new System.Windows.Forms.BindingSource(components);
            imageDataGridViewImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dataGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)employeeBindingSource).BeginInit();
            SuspendLayout();
            // 
            // textId
            // 
            textId.Location = new System.Drawing.Point(61, 317);
            textId.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textId.Name = "textId";
            textId.Size = new System.Drawing.Size(256, 23);
            textId.TabIndex = 0;
            textId.KeyPress += textId_KeyPress;
            // 
            // textName
            // 
            textName.Location = new System.Drawing.Point(61, 347);
            textName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textName.Name = "textName";
            textName.Size = new System.Drawing.Size(256, 23);
            textName.TabIndex = 1;
            // 
            // buttonRedo
            // 
            buttonRedo.Location = new System.Drawing.Point(103, 12);
            buttonRedo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonRedo.Name = "buttonRedo";
            buttonRedo.Size = new System.Drawing.Size(82, 30);
            buttonRedo.TabIndex = 2;
            buttonRedo.Text = "Redo";
            buttonRedo.UseVisualStyleBackColor = true;
            buttonRedo.Click += redo_Click;
            // 
            // buttonUndo
            // 
            buttonUndo.Location = new System.Drawing.Point(13, 12);
            buttonUndo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonUndo.Name = "buttonUndo";
            buttonUndo.Size = new System.Drawing.Size(82, 30);
            buttonUndo.TabIndex = 3;
            buttonUndo.Text = "Undo";
            buttonUndo.UseVisualStyleBackColor = true;
            buttonUndo.Click += undo_Click;
            // 
            // labelId
            // 
            labelId.AutoSize = true;
            labelId.Location = new System.Drawing.Point(13, 320);
            labelId.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelId.Name = "labelId";
            labelId.Size = new System.Drawing.Size(20, 15);
            labelId.TabIndex = 4;
            labelId.Text = "Id:";
            // 
            // labelName
            // 
            labelName.AutoSize = true;
            labelName.Location = new System.Drawing.Point(13, 350);
            labelName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelName.Name = "labelName";
            labelName.Size = new System.Drawing.Size(42, 15);
            labelName.TabIndex = 5;
            labelName.Text = "Name:";
            // 
            // dataGrid
            // 
            dataGrid.AllowUserToAddRows = false;
            dataGrid.AllowUserToDeleteRows = false;
            dataGrid.AutoGenerateColumns = false;
            dataGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { imageDataGridViewImageColumn, idDataGridViewTextBoxColumn, nameDataGridViewTextBoxColumn });
            dataGrid.DataSource = employeeBindingSource;
            dataGrid.Location = new System.Drawing.Point(12, 48);
            dataGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dataGrid.MultiSelect = false;
            dataGrid.Name = "dataGrid";
            dataGrid.ReadOnly = true;
            dataGrid.RowHeadersVisible = false;
            dataGrid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            dataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGrid.Size = new System.Drawing.Size(306, 262);
            dataGrid.TabIndex = 6;
            dataGrid.SelectionChanged += dataGrid_SelectionChanged;
            // 
            // buttonAbout
            // 
            buttonAbout.Location = new System.Drawing.Point(234, 12);
            buttonAbout.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonAbout.Name = "buttonAbout";
            buttonAbout.Size = new System.Drawing.Size(83, 30);
            buttonAbout.TabIndex = 7;
            buttonAbout.Text = "About";
            buttonAbout.UseVisualStyleBackColor = true;
            buttonAbout.Click += about_Click;
            // 
            // employeeBindingSource
            // 
            employeeBindingSource.DataSource = typeof(Employees.ViewModels.Employee);
            // 
            // imageDataGridViewImageColumn
            // 
            imageDataGridViewImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            imageDataGridViewImageColumn.DataPropertyName = "Image";
            imageDataGridViewImageColumn.HeaderText = "Image";
            imageDataGridViewImageColumn.Name = "imageDataGridViewImageColumn";
            imageDataGridViewImageColumn.ReadOnly = true;
            imageDataGridViewImageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            imageDataGridViewImageColumn.Width = 46;
            // 
            // idDataGridViewTextBoxColumn
            // 
            idDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            idDataGridViewTextBoxColumn.HeaderText = "Id";
            idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            idDataGridViewTextBoxColumn.ReadOnly = true;
            idDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            nameDataGridViewTextBoxColumn.HeaderText = "Name";
            nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            nameDataGridViewTextBoxColumn.ReadOnly = true;
            nameDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // EmployeeView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(331, 382);
            Controls.Add(buttonAbout);
            Controls.Add(dataGrid);
            Controls.Add(labelName);
            Controls.Add(labelId);
            Controls.Add(buttonUndo);
            Controls.Add(buttonRedo);
            Controls.Add(textName);
            Controls.Add(textId);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(347, 421);
            MinimumSize = new System.Drawing.Size(347, 421);
            Name = "EmployeeView";
            Text = "Employees";
            ((System.ComponentModel.ISupportInitialize)dataGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)employeeBindingSource).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox textId;
        private System.Windows.Forms.TextBox textName;
        private System.Windows.Forms.Button buttonRedo;
        private System.Windows.Forms.Button buttonUndo;
        private System.Windows.Forms.Label labelId;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.BindingSource employeeBindingSource;
        private System.Windows.Forms.DataGridViewImageColumn imageDataGridViewImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
    }
}
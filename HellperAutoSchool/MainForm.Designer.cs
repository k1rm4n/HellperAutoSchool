
namespace HellperAutoSchool
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboListTable = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboListTable
            // 
            this.comboListTable.FormattingEnabled = true;
            this.comboListTable.Items.AddRange(new object[] {
            "Студенты",
            "Учителя",
            "Инструкторы",
            "Автомобили",
            "Группы",
            "Расписание"});
            this.comboListTable.Location = new System.Drawing.Point(158, 12);
            this.comboListTable.Name = "comboListTable";
            this.comboListTable.Size = new System.Drawing.Size(121, 23);
            this.comboListTable.TabIndex = 0;
            this.comboListTable.SelectedIndexChanged += new System.EventHandler(this.ComboListTable_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(697, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Выйти";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Exit_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboListTable);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Админ панель";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboListTable;
        private System.Windows.Forms.Button button1;
    }
}


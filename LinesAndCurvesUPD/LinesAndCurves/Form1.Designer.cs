namespace LinesAndCurves
{
    partial class Form1
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
            panel1 = new Panel();
            Generate_button = new Button();
            HReflection_checkbox = new CheckBox();
            VReflection_checkbox = new CheckBox();
            Flip_checkbox = new CheckBox();
            Save_button = new Button();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Location = new Point(154, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(665, 446);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            panel1.MouseClick += panel1_MouseClick;
            // 
            // Generate_button
            // 
            Generate_button.Location = new Point(12, 21);
            Generate_button.Name = "Generate_button";
            Generate_button.Size = new Size(129, 36);
            Generate_button.TabIndex = 1;
            Generate_button.Text = "Generate curves";
            Generate_button.UseVisualStyleBackColor = true;
            Generate_button.Click += Generate_button_Click;
            // 
            // HReflection_checkbox
            // 
            HReflection_checkbox.AutoSize = true;
            HReflection_checkbox.Location = new Point(12, 63);
            HReflection_checkbox.Name = "HReflection_checkbox";
            HReflection_checkbox.Size = new Size(129, 19);
            HReflection_checkbox.TabIndex = 2;
            HReflection_checkbox.Text = "Reflect Horizontally";
            HReflection_checkbox.UseVisualStyleBackColor = true;
            HReflection_checkbox.CheckedChanged += HReflection_checkbox_CheckedChanged;
            // 
            // VReflection_checkbox
            // 
            VReflection_checkbox.AutoSize = true;
            VReflection_checkbox.Location = new Point(12, 88);
            VReflection_checkbox.Name = "VReflection_checkbox";
            VReflection_checkbox.Size = new Size(112, 19);
            VReflection_checkbox.TabIndex = 3;
            VReflection_checkbox.Text = "Reflect Vertically";
            VReflection_checkbox.UseVisualStyleBackColor = true;
            VReflection_checkbox.CheckedChanged += VReflection_checkbox_CheckedChanged;
            // 
            // Flip_checkbox
            // 
            Flip_checkbox.AutoSize = true;
            Flip_checkbox.Location = new Point(12, 113);
            Flip_checkbox.Name = "Flip_checkbox";
            Flip_checkbox.Size = new Size(45, 19);
            Flip_checkbox.TabIndex = 4;
            Flip_checkbox.Text = "Flip";
            Flip_checkbox.UseVisualStyleBackColor = true;
            // 
            // Save_button
            // 
            Save_button.Location = new Point(12, 142);
            Save_button.Name = "Save_button";
            Save_button.Size = new Size(129, 42);
            Save_button.TabIndex = 5;
            Save_button.Text = "Save to SVG";
            Save_button.UseVisualStyleBackColor = true;
            Save_button.Click += Save_button_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(831, 470);
            Controls.Add(Save_button);
            Controls.Add(Flip_checkbox);
            Controls.Add(VReflection_checkbox);
            Controls.Add(HReflection_checkbox);
            Controls.Add(Generate_button);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button Generate_button;
        private CheckBox HReflection_checkbox;
        private CheckBox VReflection_checkbox;
        private CheckBox Flip_checkbox;
        private Button Save_button;
    }
}

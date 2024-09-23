<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Mainwindow2
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Mainwindow2))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.p_monster = New System.Windows.Forms.PictureBox
        Me.T01 = New System.Windows.Forms.Button
        Me.T02 = New System.Windows.Forms.Button
        Me.T03 = New System.Windows.Forms.ComboBox
        Me.p_health2 = New System.Windows.Forms.Panel
        Me.P001 = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.P004 = New System.Windows.Forms.Panel
        Me.P003 = New System.Windows.Forms.Panel
        Me.P002 = New System.Windows.Forms.Panel
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.T04 = New System.Windows.Forms.Button
        Me.PBInventory = New System.Windows.Forms.PictureBox
        Me.T06 = New System.Windows.Forms.Button
        Me.T07 = New System.Windows.Forms.Button
        Me.lstSpells1 = New System.Windows.Forms.ListBox
        Me.bAbort = New System.Windows.Forms.Button
        Me.bCast = New System.Windows.Forms.Button
        Me.T05 = New System.Windows.Forms.Button
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.T08 = New System.Windows.Forms.Button
        Me.T09 = New System.Windows.Forms.Button
        Me.PictureBox3 = New System.Windows.Forms.PictureBox
        Me.TimeMonster = New System.Windows.Forms.Timer(Me.components)
        Me.p_effects = New System.Windows.Forms.PictureBox
        Me.TimeEffect = New System.Windows.Forms.Timer(Me.components)
        Me.TimeCharacter = New System.Windows.Forms.Timer(Me.components)
        Me.TimeBitmap = New System.Windows.Forms.Timer(Me.components)
        Me.TimeBitmap2 = New System.Windows.Forms.Timer(Me.components)
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.PictureBox5 = New System.Windows.Forms.PictureBox
        Me.cbWalls = New System.Windows.Forms.ComboBox
        Me.p_effects2 = New System.Windows.Forms.PictureBox
        Me.PictureBox6 = New System.Windows.Forms.PictureBox
        Me.PictureBox7 = New System.Windows.Forms.PictureBox
        Me.p_effects3 = New System.Windows.Forms.PictureBox
        Me.p_effects4 = New System.Windows.Forms.PictureBox
        Me.p_effects5 = New System.Windows.Forms.PictureBox
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.p_monster, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.P001.SuspendLayout()
        Me.P002.SuspendLayout()
        CType(Me.PBInventory, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.p_effects, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.p_effects2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.p_effects3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.p_effects4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.p_effects5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(111, 58)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(350, 240)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 59
        Me.PictureBox1.TabStop = False
        '
        'p_monster
        '
        Me.p_monster.Image = CType(resources.GetObject("p_monster.Image"), System.Drawing.Image)
        Me.p_monster.Location = New System.Drawing.Point(142, 1)
        Me.p_monster.Name = "p_monster"
        Me.p_monster.Size = New System.Drawing.Size(173, 171)
        Me.p_monster.TabIndex = 112
        Me.p_monster.TabStop = False
        Me.p_monster.Visible = False
        '
        'T01
        '
        Me.T01.Location = New System.Drawing.Point(12, 331)
        Me.T01.Name = "T01"
        Me.T01.Size = New System.Drawing.Size(61, 23)
        Me.T01.TabIndex = 124
        Me.T01.Text = "monster1"
        Me.T01.UseVisualStyleBackColor = True
        Me.T01.Visible = False
        '
        'T02
        '
        Me.T02.Location = New System.Drawing.Point(79, 331)
        Me.T02.Name = "T02"
        Me.T02.Size = New System.Drawing.Size(61, 23)
        Me.T02.TabIndex = 125
        Me.T02.Text = "monster2"
        Me.T02.UseVisualStyleBackColor = True
        Me.T02.Visible = False
        '
        'T03
        '
        Me.T03.FormattingEnabled = True
        Me.T03.Items.AddRange(New Object() {"gargoyle", "ant", "aservant", "basilisk", "beholder", "bullette", "cleric", "cube", "dragon", "giant", "guard", "guardian", "hellhound", "mage", "mantis", "medusa", "mindflayer", "salamander", "skeletwarrior", "snake", "spider", "wasp", "willowwisp"})
        Me.T03.Location = New System.Drawing.Point(146, 333)
        Me.T03.Name = "T03"
        Me.T03.Size = New System.Drawing.Size(73, 21)
        Me.T03.TabIndex = 171
        Me.T03.Text = "gargoyle"
        Me.T03.Visible = False
        '
        'p_health2
        '
        Me.p_health2.BackColor = System.Drawing.Color.White
        Me.p_health2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.p_health2.Location = New System.Drawing.Point(532, 292)
        Me.p_health2.Name = "p_health2"
        Me.p_health2.Size = New System.Drawing.Size(300, 10)
        Me.p_health2.TabIndex = 173
        '
        'P001
        '
        Me.P001.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.P001.Controls.Add(Me.Panel3)
        Me.P001.Controls.Add(Me.Panel4)
        Me.P001.Location = New System.Drawing.Point(543, 199)
        Me.P001.Name = "P001"
        Me.P001.Size = New System.Drawing.Size(123, 10)
        Me.P001.TabIndex = 183
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Location = New System.Drawing.Point(-2, 20)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(173, 20)
        Me.Panel3.TabIndex = 152
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Location = New System.Drawing.Point(-1, 26)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(173, 20)
        Me.Panel4.TabIndex = 150
        '
        'P004
        '
        Me.P004.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.P004.Location = New System.Drawing.Point(543, 270)
        Me.P004.Name = "P004"
        Me.P004.Size = New System.Drawing.Size(123, 10)
        Me.P004.TabIndex = 186
        '
        'P003
        '
        Me.P003.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.P003.Location = New System.Drawing.Point(543, 253)
        Me.P003.Name = "P003"
        Me.P003.Size = New System.Drawing.Size(123, 10)
        Me.P003.TabIndex = 185
        '
        'P002
        '
        Me.P002.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.P002.Controls.Add(Me.Panel5)
        Me.P002.Controls.Add(Me.Panel2)
        Me.P002.Location = New System.Drawing.Point(543, 233)
        Me.P002.Name = "P002"
        Me.P002.Size = New System.Drawing.Size(123, 10)
        Me.P002.TabIndex = 184
        '
        'Panel5
        '
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Location = New System.Drawing.Point(-2, 20)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(173, 20)
        Me.Panel5.TabIndex = 152
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Location = New System.Drawing.Point(-1, 26)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(173, 20)
        Me.Panel2.TabIndex = 150
        '
        'T04
        '
        Me.T04.Location = New System.Drawing.Point(225, 333)
        Me.T04.Name = "T04"
        Me.T04.Size = New System.Drawing.Size(39, 23)
        Me.T04.TabIndex = 195
        Me.T04.Text = "off"
        Me.T04.UseVisualStyleBackColor = True
        Me.T04.Visible = False
        '
        'PBInventory
        '
        Me.PBInventory.Image = CType(resources.GetObject("PBInventory.Image"), System.Drawing.Image)
        Me.PBInventory.Location = New System.Drawing.Point(142, 1)
        Me.PBInventory.Name = "PBInventory"
        Me.PBInventory.Size = New System.Drawing.Size(77, 347)
        Me.PBInventory.TabIndex = 196
        Me.PBInventory.TabStop = False
        '
        'T06
        '
        Me.T06.Location = New System.Drawing.Point(322, 366)
        Me.T06.Name = "T06"
        Me.T06.Size = New System.Drawing.Size(37, 23)
        Me.T06.TabIndex = 197
        Me.T06.Text = "INV"
        Me.T06.UseVisualStyleBackColor = True
        Me.T06.Visible = False
        '
        'T07
        '
        Me.T07.Location = New System.Drawing.Point(365, 366)
        Me.T07.Name = "T07"
        Me.T07.Size = New System.Drawing.Size(37, 23)
        Me.T07.TabIndex = 198
        Me.T07.Text = "INV2"
        Me.T07.UseVisualStyleBackColor = True
        Me.T07.Visible = False
        '
        'lstSpells1
        '
        Me.lstSpells1.BackColor = System.Drawing.Color.Gray
        Me.lstSpells1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstSpells1.ForeColor = System.Drawing.Color.White
        Me.lstSpells1.FormattingEnabled = True
        Me.lstSpells1.Items.AddRange(New Object() {"BURNING HANDS", "BURNING HANDS", "BURNING HANDS", "BURNING HANDS", "BURNING HANDS", "BURNING HANDS", "BURNING HANDS", "BURNING HANDS"})
        Me.lstSpells1.Location = New System.Drawing.Point(293, 354)
        Me.lstSpells1.Name = "lstSpells1"
        Me.lstSpells1.Size = New System.Drawing.Size(160, 43)
        Me.lstSpells1.TabIndex = 200
        Me.lstSpells1.Visible = False
        '
        'bAbort
        '
        Me.bAbort.Location = New System.Drawing.Point(420, 367)
        Me.bAbort.Name = "bAbort"
        Me.bAbort.Size = New System.Drawing.Size(77, 20)
        Me.bAbort.TabIndex = 211
        Me.bAbort.Text = "ABORT SPELL"
        Me.bAbort.UseVisualStyleBackColor = True
        Me.bAbort.Visible = False
        '
        'bCast
        '
        Me.bCast.Location = New System.Drawing.Point(408, 355)
        Me.bCast.Name = "bCast"
        Me.bCast.Size = New System.Drawing.Size(89, 20)
        Me.bCast.TabIndex = 212
        Me.bCast.Text = "CAST"
        Me.bCast.UseVisualStyleBackColor = True
        Me.bCast.Visible = False
        '
        'T05
        '
        Me.T05.Location = New System.Drawing.Point(270, 333)
        Me.T05.Name = "T05"
        Me.T05.Size = New System.Drawing.Size(37, 23)
        Me.T05.TabIndex = 213
        Me.T05.Text = "spell"
        Me.T05.UseVisualStyleBackColor = True
        Me.T05.Visible = False
        '
        'RichTextBox1
        '
        Me.RichTextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox1.Location = New System.Drawing.Point(5, 354)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.Size = New System.Drawing.Size(571, 45)
        Me.RichTextBox1.TabIndex = 215
        Me.RichTextBox1.Text = ""
        '
        'PictureBox2
        '
        Me.PictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox2.Location = New System.Drawing.Point(342, 262)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(119, 69)
        Me.PictureBox2.TabIndex = 216
        Me.PictureBox2.TabStop = False
        '
        'T08
        '
        Me.T08.Location = New System.Drawing.Point(322, 354)
        Me.T08.Name = "T08"
        Me.T08.Size = New System.Drawing.Size(37, 23)
        Me.T08.TabIndex = 218
        Me.T08.Text = "spell"
        Me.T08.UseVisualStyleBackColor = True
        Me.T08.Visible = False
        '
        'T09
        '
        Me.T09.Location = New System.Drawing.Point(365, 354)
        Me.T09.Name = "T09"
        Me.T09.Size = New System.Drawing.Size(37, 23)
        Me.T09.TabIndex = 219
        Me.T09.Text = "spell"
        Me.T09.UseVisualStyleBackColor = True
        Me.T09.Visible = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Enabled = False
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(293, 282)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(22, 20)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox3.TabIndex = 220
        Me.PictureBox3.TabStop = False
        Me.PictureBox3.Visible = False
        '
        'TimeMonster
        '
        Me.TimeMonster.Interval = 3000
        '
        'p_effects
        '
        Me.p_effects.Image = CType(resources.GetObject("p_effects.Image"), System.Drawing.Image)
        Me.p_effects.Location = New System.Drawing.Point(408, 165)
        Me.p_effects.Name = "p_effects"
        Me.p_effects.Size = New System.Drawing.Size(169, 151)
        Me.p_effects.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.p_effects.TabIndex = 221
        Me.p_effects.TabStop = False
        Me.p_effects.Visible = False
        '
        'TimeEffect
        '
        Me.TimeEffect.Interval = 500
        '
        'TimeCharacter
        '
        Me.TimeCharacter.Interval = 500
        '
        'TimeBitmap
        '
        Me.TimeBitmap.Interval = 500
        '
        'TimeBitmap2
        '
        Me.TimeBitmap2.Interval = 500
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(322, 247)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(160, 101)
        Me.PictureBox4.TabIndex = 222
        Me.PictureBox4.TabStop = False
        '
        'PictureBox5
        '
        Me.PictureBox5.Enabled = False
        Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
        Me.PictureBox5.Location = New System.Drawing.Point(293, 306)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(22, 20)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox5.TabIndex = 223
        Me.PictureBox5.TabStop = False
        Me.PictureBox5.Visible = False
        '
        'cbWalls
        '
        Me.cbWalls.FormattingEnabled = True
        Me.cbWalls.Items.AddRange(New Object() {"Silver", "Temple", "Catacombs", "Wood", "Sewers", "Dwarven", "Drow", "Sanctum", "Green", "Crimson", "Mage"})
        Me.cbWalls.Location = New System.Drawing.Point(12, 310)
        Me.cbWalls.Name = "cbWalls"
        Me.cbWalls.Size = New System.Drawing.Size(106, 21)
        Me.cbWalls.TabIndex = 224
        Me.cbWalls.Visible = False
        '
        'p_effects2
        '
        Me.p_effects2.Image = CType(resources.GetObject("p_effects2.Image"), System.Drawing.Image)
        Me.p_effects2.Location = New System.Drawing.Point(408, 165)
        Me.p_effects2.Name = "p_effects2"
        Me.p_effects2.Size = New System.Drawing.Size(169, 151)
        Me.p_effects2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.p_effects2.TabIndex = 225
        Me.p_effects2.TabStop = False
        Me.p_effects2.Visible = False
        '
        'PictureBox6
        '
        Me.PictureBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBox6.Image = CType(resources.GetObject("PictureBox6.Image"), System.Drawing.Image)
        Me.PictureBox6.Location = New System.Drawing.Point(322, 231)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(240, 140)
        Me.PictureBox6.TabIndex = 226
        Me.PictureBox6.TabStop = False
        Me.PictureBox6.Visible = False
        '
        'PictureBox7
        '
        Me.PictureBox7.Image = CType(resources.GetObject("PictureBox7.Image"), System.Drawing.Image)
        Me.PictureBox7.Location = New System.Drawing.Point(282, 202)
        Me.PictureBox7.Name = "PictureBox7"
        Me.PictureBox7.Size = New System.Drawing.Size(320, 202)
        Me.PictureBox7.TabIndex = 227
        Me.PictureBox7.TabStop = False
        Me.PictureBox7.Visible = False
        '
        'p_effects3
        '
        Me.p_effects3.Image = CType(resources.GetObject("p_effects3.Image"), System.Drawing.Image)
        Me.p_effects3.Location = New System.Drawing.Point(408, 165)
        Me.p_effects3.Name = "p_effects3"
        Me.p_effects3.Size = New System.Drawing.Size(169, 151)
        Me.p_effects3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.p_effects3.TabIndex = 228
        Me.p_effects3.TabStop = False
        Me.p_effects3.Visible = False
        '
        'p_effects4
        '
        Me.p_effects4.Image = CType(resources.GetObject("p_effects4.Image"), System.Drawing.Image)
        Me.p_effects4.Location = New System.Drawing.Point(408, 165)
        Me.p_effects4.Name = "p_effects4"
        Me.p_effects4.Size = New System.Drawing.Size(169, 151)
        Me.p_effects4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.p_effects4.TabIndex = 229
        Me.p_effects4.TabStop = False
        Me.p_effects4.Visible = False
        '
        'p_effects5
        '
        Me.p_effects5.Image = CType(resources.GetObject("p_effects5.Image"), System.Drawing.Image)
        Me.p_effects5.Location = New System.Drawing.Point(408, 165)
        Me.p_effects5.Name = "p_effects5"
        Me.p_effects5.Size = New System.Drawing.Size(169, 151)
        Me.p_effects5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.p_effects5.TabIndex = 230
        Me.p_effects5.TabStop = False
        Me.p_effects5.Visible = False
        '
        'Mainwindow2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(644, 401)
        Me.Controls.Add(Me.p_effects5)
        Me.Controls.Add(Me.p_effects4)
        Me.Controls.Add(Me.p_effects3)
        Me.Controls.Add(Me.PictureBox6)
        Me.Controls.Add(Me.PictureBox7)
        Me.Controls.Add(Me.p_effects2)
        Me.Controls.Add(Me.PBInventory)
        Me.Controls.Add(Me.cbWalls)
        Me.Controls.Add(Me.PictureBox5)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.p_effects)
        Me.Controls.Add(Me.PictureBox3)
        Me.Controls.Add(Me.T09)
        Me.Controls.Add(Me.T08)
        Me.Controls.Add(Me.T05)
        Me.Controls.Add(Me.bCast)
        Me.Controls.Add(Me.bAbort)
        Me.Controls.Add(Me.T07)
        Me.Controls.Add(Me.T06)
        Me.Controls.Add(Me.T04)
        Me.Controls.Add(Me.P004)
        Me.Controls.Add(Me.P003)
        Me.Controls.Add(Me.P002)
        Me.Controls.Add(Me.P001)
        Me.Controls.Add(Me.p_health2)
        Me.Controls.Add(Me.T03)
        Me.Controls.Add(Me.T02)
        Me.Controls.Add(Me.T01)
        Me.Controls.Add(Me.p_monster)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lstSpells1)
        Me.Controls.Add(Me.RichTextBox1)
        Me.Controls.Add(Me.PictureBox4)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Mainwindow2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Darkmoon RPG"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.p_monster, System.ComponentModel.ISupportInitialize).EndInit()
        Me.P001.ResumeLayout(False)
        Me.P002.ResumeLayout(False)
        CType(Me.PBInventory, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.p_effects, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.p_effects2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox7, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.p_effects3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.p_effects4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.p_effects5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents p_monster As System.Windows.Forms.PictureBox
    Friend WithEvents T01 As System.Windows.Forms.Button
    Friend WithEvents T02 As System.Windows.Forms.Button
    Friend WithEvents T03 As System.Windows.Forms.ComboBox
    Friend WithEvents p_health2 As System.Windows.Forms.Panel
    Friend WithEvents P001 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents P004 As System.Windows.Forms.Panel
    Friend WithEvents P003 As System.Windows.Forms.Panel
    Friend WithEvents P002 As System.Windows.Forms.Panel
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents T04 As System.Windows.Forms.Button
    Friend WithEvents PBInventory As System.Windows.Forms.PictureBox
    Friend WithEvents T06 As System.Windows.Forms.Button
    Friend WithEvents T07 As System.Windows.Forms.Button
    Friend WithEvents lstSpells1 As System.Windows.Forms.ListBox
    Friend WithEvents bAbort As System.Windows.Forms.Button
    Friend WithEvents bCast As System.Windows.Forms.Button
    Friend WithEvents T05 As System.Windows.Forms.Button
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents T08 As System.Windows.Forms.Button
    Friend WithEvents T09 As System.Windows.Forms.Button
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents TimeMonster As System.Windows.Forms.Timer
    Friend WithEvents p_effects As System.Windows.Forms.PictureBox
    Friend WithEvents TimeEffect As System.Windows.Forms.Timer
    Friend WithEvents TimeCharacter As System.Windows.Forms.Timer
    Friend WithEvents TimeBitmap As System.Windows.Forms.Timer
    Friend WithEvents TimeBitmap2 As System.Windows.Forms.Timer
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents cbWalls As System.Windows.Forms.ComboBox
    Friend WithEvents p_effects2 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox7 As System.Windows.Forms.PictureBox
    Friend WithEvents p_effects3 As System.Windows.Forms.PictureBox
    Friend WithEvents p_effects4 As System.Windows.Forms.PictureBox
    Friend WithEvents p_effects5 As System.Windows.Forms.PictureBox
End Class

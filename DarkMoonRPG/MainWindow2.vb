Option Explicit On
Imports System.IO
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.Common

Public Class Mainwindow2

    Public MyMDI As MDI

    Private Declare Unicode Function LoadCursorFromFile Lib "user32.dll" Alias "LoadCursorFromFileW" (ByVal filename As String) As IntPtr
    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Long) As Integer

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        'detect up arrow key

        If keyData = Keys.Home Or keyData = Keys.Q Then
            Me.ClickLeftturn()
            Return True
        End If
        If keyData = Keys.Prior Or keyData = Keys.E Then
            Me.ClickRightturn()
            Return True
        End If

        If keyData = Keys.Up Or keyData = Keys.W Then
            Me.ClickUp()
            Return True
        End If
        'detect down arrow key
        If keyData = Keys.Down Or keyData = Keys.S Then
            Me.ClickDown()
            Return True
        End If
        'detect left arrow key
        If keyData = Keys.Left Or keyData = Keys.A Then
            Me.ClickLeft()
            Return True
        End If
        'detect right arrow key
        If keyData = Keys.Right Or keyData = Keys.D Then
            Me.ClickRight()
            Return True
        End If

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Public Sub LogMessage(ByVal msg As String, ByVal program As String)

        Dim myfile As String
        myfile = CurDir() + "\log\system.logging"

        Dim FS As New FileStream(myfile, FileMode.Open, FileAccess.ReadWrite)
        Dim SR As New StreamWriter(FS)

        FS.Seek(0, SeekOrigin.End)
        SR.WriteLine("Error: " + Now() + " , Program: " + program)
        SR.WriteLine(msg)

        SR.Close()
        FS.Close()

    End Sub


    Private Sub UpdateInventories()

        For i = 0 To 3
            GameChars(i).StoreInventory(i + 1)

        Next
    End Sub

    Private Sub Mainwindow2_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Dim rc As Integer

        rc = MsgBox("Do you want to save your inventory ?", MsgBoxStyle.YesNo)
        If rc = MsgBoxResult.Yes Then
            Me.UpdateInventories()
        End If

        'Me.Close()
        'MyMDI.Close()
        'Stop
        Application.Exit()


    End Sub



    Protected Overrides Sub OnLocationChanged(ByVal e As EventArgs)
        Static loc As Point = Me.Location
        'Me.Location = loc

        Me.Location = New Point(0, 0)

    End Sub

    Public Sub New(ByRef MyForm As MDI)



        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        MyMDI = MyForm


    End Sub


    Private Sub MainWindow2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim bmp1, bmp2 As Bitmap
        Try

            GameFile = CurDir()
            dsUsers.ReadXml(GameFile + "\xml\users.xml")

            BackdropId = Backdrops(CurrentLevel)
            WallsId = Walls(CurrentLevel)
            StairsId = Stairs(CurrentLevel)
            DoorId = Doors(CurrentLevel)

            Me.LoadDatasets()
            Me.FillGameFieldControls()
            Me.FillWalltypeSettings()

            Me.InitializeBitmaps()
            Me.FillMapsandItems()

            'Dim bmp1, bmp2 As Bitmap
            bmp1 = Me.GetBackdrop(BackdropId)
            PictureBox1.Image = bmp1 'GameField 'bmp1

            PictureBox1.Location = New Point(MainWindow_xpos, MainWindow_ypos)
            PictureBox1.Width = MainWindow_width
            PictureBox1.Height = MainWindow_height

            bmp2 = Bitmap.FromFile(GameFile + "\gamefield\inventory1.bmp")
            PBInventory.Image = bmp2
            PBInventory.Location = New Point(Inventory_xpos, Inventory_ypos)
            PBInventory.Width = 77 'Inventory_width
            PBInventory.Height = Inventory_height

            Me.CreateControls()
            Me.PositionControls()

            'Me.MakeControlsVisible(False)
            PBInventory.BringToFront()
            Me.CreateInventory()
            Me.RepositionInventory("right")


            'HEALTHPIC(0).Width = HEALTHPIC(0).Width * 0.8
            'HEALTHMONSTER.Width = HEALTHMONSTER.Width * 0.8

            CurrentPosX3 = CurrentPosX + 3
            CurrentPosY3 = CurrentPosY + 3
            p_monster.SizeMode = PictureBoxSizeMode.StretchImage

            Me.PaintWalls()
            Me.PaintItems()

            Me.Characters()
            Me.FillMonstersDB()


            'specific code for Level Wood
            Me.ClickRightturn()
            Me.ClickRightturn()
            'specific code for Level Wood

            Me.Location = New Point(0, 0)
            Me.Text = "                                                                                       " + "Darkmoon RPG"

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "MainWindow_Load")


        End Try



    End Sub

    Public Function LoadPortrait(ByVal pnr As Integer) As Bitmap
        Dim pictname As String
        Dim pict As Bitmap

        Try
            pictname = "\chars\character00" + pnr.ToString() + ".png"
            pict = Bitmap.FromFile(GameFile + pictname)
            Return pict

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadPortrait(" + pnr.ToString() + ")")

        End Try

    End Function

    Public Function LoadDeath(ByVal pnr As Integer) As Bitmap
        Dim pictname As String
        Dim pict As Bitmap

        Try
            pictname = "\chars\Death.bmp"
            pict = Bitmap.FromFile(GameFile + pictname)

            Return pict

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadDeath" + pnr.ToString() + ")")

        End Try

    End Function

    Private Function GetBackdrop(ByVal bnr As Integer) As Bitmap
        Dim bmp As Bitmap

        Try
            GameFile = CurDir()
            bmp = Bitmap.FromFile(GameFile + "\backdrops\BACKDROP" + bnr.ToString() + ".bmp")

            Return bmp

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "GetBackdrop" + bnr.ToString() + ")")

        End Try

    End Function


    Private Sub FillWalltypeSettings()
        Dim rownr As Integer = 0
        Dim x, y, w, h, dx, dy As Integer

        Try

            'Main Wall Level 1,2,3 Settings
            MW_lvl1_xpos = dsWallTypes.Tables(0).Rows(rownr).Item(2)
            MW_lvl1_ypos = dsWallTypes.Tables(0).Rows(rownr).Item(3)
            MW_lvl1_width = dsWallTypes.Tables(0).Rows(rownr).Item(4)
            MW_lvl1_height = dsWallTypes.Tables(0).Rows(rownr).Item(5)
            MW_lvl2_xpos = dsWallTypes.Tables(0).Rows(rownr).Item(6)
            MW_lvl2_ypos = dsWallTypes.Tables(0).Rows(rownr).Item(7)
            MW_lvl2_width = dsWallTypes.Tables(0).Rows(rownr).Item(8)
            MW_lvl2_height = dsWallTypes.Tables(0).Rows(rownr).Item(9)
            MW_lvl3_xpos = dsWallTypes.Tables(0).Rows(rownr).Item(10)
            MW_lvl3_ypos = dsWallTypes.Tables(0).Rows(rownr).Item(11)
            MW_lvl3_width = dsWallTypes.Tables(0).Rows(rownr).Item(12)
            MW_lvl3_height = dsWallTypes.Tables(0).Rows(rownr).Item(13)
            'Side Walls Level 1,2,3,4 Settings
            SWL_lvl1_xpos = dsWallTypes.Tables(0).Rows(rownr).Item(14)
            SWL_lvl1_ypos = dsWallTypes.Tables(0).Rows(rownr).Item(15)
            SWR_lvl1_xpos = dsWallTypes.Tables(0).Rows(rownr).Item(16)
            SWR_lvl1_ypos = SWL_lvl1_ypos
            SWL_lvl1_width = dsWallTypes.Tables(0).Rows(rownr).Item(17)
            SWL_lvl1_height = dsWallTypes.Tables(0).Rows(rownr).Item(18)
            SWL_lvl2_xpos = dsWallTypes.Tables(0).Rows(rownr).Item(19)
            SWL_lvl2_ypos = dsWallTypes.Tables(0).Rows(rownr).Item(20)
            SWR_lvl2_xpos = dsWallTypes.Tables(0).Rows(rownr).Item(21)
            SWR_lvl2_ypos = SWL_lvl2_ypos
            SWL_lvl2_width = dsWallTypes.Tables(0).Rows(rownr).Item(22)
            SWL_lvl2_height = dsWallTypes.Tables(0).Rows(rownr).Item(23)
            SWL_lvl3_xpos = dsWallTypes.Tables(0).Rows(rownr).Item(24)
            SWL_lvl3_ypos = dsWallTypes.Tables(0).Rows(rownr).Item(25)
            SWR_lvl3_xpos = dsWallTypes.Tables(0).Rows(rownr).Item(26)
            SWR_lvl3_ypos = SWL_lvl3_ypos
            SWL_lvl3_width = dsWallTypes.Tables(0).Rows(rownr).Item(27)
            SWL_lvl3_height = dsWallTypes.Tables(0).Rows(rownr).Item(28)
            SWL_lvl4_xpos = dsWallTypes.Tables(0).Rows(rownr).Item(29)
            SWL_lvl4_ypos = dsWallTypes.Tables(0).Rows(rownr).Item(30)
            SWR_lvl4_xpos = dsWallTypes.Tables(0).Rows(rownr).Item(31)
            SWR_lvl4_ypos = SWL_lvl4_ypos
            SWL_lvl4_width = dsWallTypes.Tables(0).Rows(rownr).Item(32)
            SWL_lvl4_height = dsWallTypes.Tables(0).Rows(rownr).Item(33)


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "FillWalltypeSettings")

        End Try


    End Sub


    Private Sub FillGameFieldControls()
        Dim rownr As Integer = 0
        Dim x, y, w, h, dx, dy As Integer
        Dim pictname As String

        Try

            'Main Game Window
            MainWindow_xpos = dsGameField.Tables(0).Rows(rownr).Item(3)
            MainWindow_ypos = dsGameField.Tables(0).Rows(rownr).Item(4)
            MainWindow_width = dsGameField.Tables(0).Rows(rownr).Item(5)
            MainWindow_height = dsGameField.Tables(0).Rows(rownr).Item(6)
            'background image
            pictname = dsGameField.Tables(0).Rows(rownr).Item(2)
            Me.BackgroundImage = Bitmap.FromFile(GameFile + "\gamefield\" + pictname)

            'text control
            Text_xpos = dsGameField.Tables(0).Rows(rownr).Item(7)
            Text_ypos = dsGameField.Tables(0).Rows(rownr).Item(8)
            Text_width = dsGameField.Tables(0).Rows(rownr).Item(9)
            Text_height = dsGameField.Tables(0).Rows(rownr).Item(10)
            Text_xpos = 5
            Text_ypos = 354
            Text_height = 45

            '5,334

            'inventory control
            x = dsGameField.Tables(0).Rows(rownr).Item(11)
            y = dsGameField.Tables(0).Rows(rownr).Item(12)
            w = dsGameField.Tables(0).Rows(rownr).Item(13)
            h = dsGameField.Tables(0).Rows(rownr).Item(14)
            Inventory_xpos = x
            Inventory_ypos = y
            Inventory_width = w
            Inventory_height = h

            'inventory items
            x = dsGameField.Tables(0).Rows(rownr).Item(15)
            y = dsGameField.Tables(0).Rows(rownr).Item(16)
            w = dsGameField.Tables(0).Rows(rownr).Item(17)
            h = dsGameField.Tables(0).Rows(rownr).Item(18)
            dx = dsGameField.Tables(0).Rows(rownr).Item(19)
            dy = dsGameField.Tables(0).Rows(rownr).Item(20)
            inv_xpos = x
            inv_ypos = y
            inv_width = w
            inv_height = h
            inv_horizontal = dx
            inv_vertical = dy
            'inventory portrait + name
            invuser_xpos = dsGameField.Tables(0).Rows(rownr).Item(21)
            invuser_ypos = dsGameField.Tables(0).Rows(rownr).Item(22)
            invuser_width = dsGameField.Tables(0).Rows(rownr).Item(23)
            invuser_height = dsGameField.Tables(0).Rows(rownr).Item(24)
            invname_xpos = dsGameField.Tables(0).Rows(rownr).Item(25)
            invname_ypos = dsGameField.Tables(0).Rows(rownr).Item(26)
            invname_width = dsGameField.Tables(0).Rows(rownr).Item(27)
            invname_height = dsGameField.Tables(0).Rows(rownr).Item(28)

            'direction arrows
            x = dsGameField.Tables(0).Rows(rownr).Item(29)
            y = dsGameField.Tables(0).Rows(rownr).Item(30)
            w = dsGameField.Tables(0).Rows(rownr).Item(31)
            h = dsGameField.Tables(0).Rows(rownr).Item(32)
            dx = dsGameField.Tables(0).Rows(rownr).Item(33)
            dy = dsGameField.Tables(0).Rows(rownr).Item(34)
            leftturn_xpos = x
            leftturn_ypos = y
            leftturn_width = w
            leftturn_height = h
            left_xpos = x
            left_ypos = y + h
            up_xpos = x + w
            up_ypos = y
            down_xpos = x + w
            down_ypos = y + h
            rightturn_xpos = x + w + w
            rightturn_ypos = y
            right_xpos = x + w + w
            right_ypos = y + h
            'Names Users
            x = dsGameField.Tables(0).Rows(rownr).Item(35)
            y = dsGameField.Tables(0).Rows(rownr).Item(36)
            w = dsGameField.Tables(0).Rows(rownr).Item(37)
            h = dsGameField.Tables(0).Rows(rownr).Item(38)
            dx = dsGameField.Tables(0).Rows(rownr).Item(39)
            dy = dsGameField.Tables(0).Rows(rownr).Item(40)
            names_xpos = x
            names_ypos = y
            names_width = w
            names_height = h
            names_horizontal = dx
            names_vertical = dy
            'Portraits Users
            x = dsGameField.Tables(0).Rows(rownr).Item(41)
            y = dsGameField.Tables(0).Rows(rownr).Item(42)
            w = dsGameField.Tables(0).Rows(rownr).Item(43)
            h = dsGameField.Tables(0).Rows(rownr).Item(44)
            dx = dsGameField.Tables(0).Rows(rownr).Item(45)
            dy = dsGameField.Tables(0).Rows(rownr).Item(46)
            portrait_xpos = x
            portrait_ypos = y
            portrait_width = w
            portrait_height = h
            portrait_horizontal = dx
            portrait_vertical = dy
            'Weapons1 Users
            x = dsGameField.Tables(0).Rows(rownr).Item(47)
            y = dsGameField.Tables(0).Rows(rownr).Item(48)
            w = dsGameField.Tables(0).Rows(rownr).Item(49)
            h = dsGameField.Tables(0).Rows(rownr).Item(50)
            dx = dsGameField.Tables(0).Rows(rownr).Item(51)
            dy = dsGameField.Tables(0).Rows(rownr).Item(52)
            hand1pic_xpos = x
            hand1pic_ypos = y
            hand1pic_width = w
            hand1pic_height = h
            hand1pic_horizontal = dx
            hand1pic_vertical = dy
            'Weapons2 Users
            x = dsGameField.Tables(0).Rows(rownr).Item(53)
            y = dsGameField.Tables(0).Rows(rownr).Item(54)
            w = dsGameField.Tables(0).Rows(rownr).Item(55)
            h = dsGameField.Tables(0).Rows(rownr).Item(56)
            dx = dsGameField.Tables(0).Rows(rownr).Item(57)
            dy = dsGameField.Tables(0).Rows(rownr).Item(58)
            hand2pic_xpos = x
            hand2pic_ypos = y
            hand2pic_width = w
            hand2pic_height = h
            hand2pic_horizontal = dx
            hand2pic_vertical = dy
            'Health Users
            x = dsGameField.Tables(0).Rows(rownr).Item(59)
            y = dsGameField.Tables(0).Rows(rownr).Item(60)
            w = dsGameField.Tables(0).Rows(rownr).Item(61)
            h = dsGameField.Tables(0).Rows(rownr).Item(62)
            dx = dsGameField.Tables(0).Rows(rownr).Item(63)
            dy = dsGameField.Tables(0).Rows(rownr).Item(64)
            healthpic_xpos = x
            healthpic_ypos = y
            healthpic_width = w
            healthpic_height = h
            healthpic_horizontal = dx
            healthpic_vertical = dy

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "FillGameFieldControls")

        End Try

    End Sub


    Private Sub FillMapsandItems()

        Try
            Me.FillMapsDB(CurrentLevel)
            Me.FillItemsDB(CurrentLevel)
            Me.FillItemsWallDB(CurrentLevel)
            Me.FillMonstersDB(CurrentLevel)

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "FillMapsandItems")

        End Try

    End Sub

    Private Sub ClearCurrentPosition()

        Try
            'PictureBox2.Image = SMALL_MAP
            PictureBox2.Image.Dispose()
            PictureBox2.Image = Bitmap.FromFile(GameFile + "\picts\bg-map.bmp")
            Me.Refresh()
            'BG_LIGHTPURPLE = Bitmap.FromFile(GameFile + "\picts\bg-lightpurple.bmp")

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClearCurrentPosition")

        End Try


    End Sub

    Private Sub WriteCurrentPosition()
        Dim x, x2, y, y2 As Integer
        Dim bmp1, bmp2 As Bitmap

        Try

            x = CurrentPosX * 3
            y = CurrentPosY * 3
            x2 = LastPosX * 3
            y2 = lastPosY * 3
            bmp1 = PictureBox2.Image
            For i = 1 To 3
                For j = 1 To 2
                    If LastPosX > 0 Then
                        bmp1.SetPixel(x2, y2, Color.White)
                    End If
                    bmp1.SetPixel(x, y, Color.Black)
                    x = x + 1
                    x2 = x2 + 1
                Next
                x = CurrentPosX * 3
                x2 = LastPosX * 3
                y = y + 1
                y2 = y2 + 1
            Next
            PictureBox2.Image = bmp1

            x = CurrentPosX * 6
            y = CurrentPosY * 6
            x2 = LastPosX * 6
            y2 = lastPosY * 6
            bmp1 = PictureBox6.Image
            For i = 1 To 6
                For j = 1 To 5
                    If LastPosX > 0 Then
                        bmp1.SetPixel(x2, y2, Color.White)
                    End If
                    bmp1.SetPixel(x, y, Color.Black)
                    x = x + 1
                    x2 = x2 + 1
                Next
                x = CurrentPosX * 6
                x2 = LastPosX * 6
                y = y + 1
                y2 = y2 + 1
            Next
            PictureBox6.Image = bmp1

            LastPosX = CurrentPosX
            lastPosY = CurrentPosY

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "WriteCurrentPosition")

        End Try


    End Sub


    Private Sub Characters()
        Dim pname, uname As String

        Try

            For i = 0 To 3
                uname = dsUsers.Tables(0).Rows(i).Item(0)
                GameChars(i) = New Character(Me, uname, "M", "Human", "Fighter", i + 1, i + 1)
                PBPortrait(i).Image = Me.LoadPortrait(GameChars(i).Picnr)
                PBNames(i).Text = uname
            Next


            Me.Cursor = Windows.Forms.Cursors.Arrow

            CurrentMessage = "Hi there all you brave valiant warriors. Welcome to the Temple of Dark Moon !!!" + vbCrLf
            CurrentMessage = CurrentMessage + "You will find awesome monsters hidden deeply in terrifying ancient dungeons." '+ vbCrLf
            CurrentMessage = CurrentMessage + "We hope you will enjoy yourselves and please practice safe sex with the monsters"

            'RichTextBox1.Enabled = False
            RichTextBox1.SelectionAlignment = HorizontalAlignment.Center
            RichTextBox1.Text = CurrentMessage


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "Characters")


        End Try

    End Sub

    Public Sub CreateInventory()
        Dim linv, luser, lname As Point

        Try

            linv.X = inv_xpos '+ Inventory_width
            linv.Y = inv_ypos
            For i As Integer = 0 To 13
                'Inventory of the character as Picturebox
                PBInv(i) = New PictureBox
                PBInv(i).Location = linv
                PBInv(i).Width = inv_width
                PBInv(i).Height = inv_height
                PBInv(i).Name = "I" + i.ToString()
                PBInv(i).SizeMode = PictureBoxSizeMode.StretchImage
                PBInv(i).BackColor = Color.Transparent
                PBInv(i).BackgroundImage = EMPTYINV
                PBInv(i).Image = EMPTYINV
                Me.Controls.Add(PBInv(i))
                PBInv(i).BringToFront()
                AddHandler PBInv(i).Click, AddressOf Me.ClickInventory

                If ((i + 1) Mod 2) = 0 Then
                    linv.X = inv_xpos '+ Inventory_width
                    linv.Y = linv.Y + inv_vertical
                Else
                    linv.X = linv.X + inv_horizontal

                End If

            Next
            luser.X = invuser_xpos '+ Inventory_width
            luser.Y = invuser_ypos
            PBInvUser = New PictureBox
            PBInvUser.Location = luser
            PBInvUser.Width = invuser_width
            PBInvUser.Height = invuser_height
            PBInvUser.Name = "INVUSER"
            PBInvUser.SizeMode = PictureBoxSizeMode.StretchImage
            PBInvUser.BackColor = Color.Transparent
            PBInvUser.Image = EMPTYINV
            Me.Controls.Add(PBInvUser)
            PBInvUser.BringToFront()
            AddHandler PBInvUser.Click, AddressOf Me.ClickInvUser

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "CreateInventory")

        End Try



    End Sub

    Private Sub ClickInvUser(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim str As String
        Dim btn As PictureBox

        Try

            btn = CType(sender, PictureBox)
            str = btn.Name
            'MsgBox("you clicked button: " + str)

            Me.RepositionInventory("right")
            InvVisible = False

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClickInvUser")

        End Try


    End Sub

    Private Sub ClickInventory(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim str As String
        Dim ind As Integer
        Dim btn As PictureBox
        Dim bmp As Bitmap

        Try

            btn = CType(sender, PictureBox)
            str = btn.Name
            If str.Length = 2 Then
                str = str.Substring(1, 1)
            Else
                str = str.Substring(1, 2)
            End If
            ind = CInt(str)

            If DragDropStarted = False Then
                SourceDragControl = PBInv(ind)
                DraggedItem = GameChars(CurChar - 1).GetInventory(ind)
                bmp = Me.Getitem(DraggedItem)
                Me.NewCursor(bmp, 30, 30)
                'Me.NewCursor(SWORD, 30, 30)
                GameChars(CurChar - 1).UpdateInventory(ind, 0)
                GameChars(CurChar - 1).ShowInventory(ind)
                DragDropStarted = True
            Else 'Dragdropactive item is dropped here
                GameChars(CurChar - 1).UpdateInventory(ind, DraggedItem)
                GameChars(CurChar - 1).ShowInventory(ind)
                Me.Cursor = Windows.Forms.Cursors.Arrow
                DragDropStarted = False
            End If
            'Me.Refresh()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClickInventory(" + ind.ToString() + ")")

        End Try

    End Sub


    Public Sub MakeControlsVisible(ByVal pboolean As Boolean)

        Try

            For i = 0 To 3
                PBNames(i).Visible = pboolean
                PBPortrait(i).Visible = pboolean
                HAND1PIC(i).Visible = pboolean
                HAND2PIC(i).Visible = pboolean
                HEALTHPIC(i).Visible = pboolean
            Next

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "MakeControlsVisible")

        End Try


    End Sub


    Public Sub CreateControls()
        Dim mage As Boolean = False
        Dim lname, lportrait, lhealth, lhand1, lhand2, lmana As Point

        Try

            lname.X = names_xpos
            lname.Y = names_ypos
            lportrait.X = portrait_xpos
            lportrait.Y = portrait_ypos
            lhealth.X = healthpic_xpos
            lhealth.Y = healthpic_ypos

            lmana.X = manapic_xpos
            lmana.Y = manapic_ypos
            lhand1.X = hand1pic_xpos
            lhand1.Y = hand1pic_ypos
            lhand2.X = hand2pic_xpos
            lhand2.Y = hand2pic_ypos

            For i As Integer = 0 To 3
                'Names of the characters as Labels
                PBNames(i) = New Label
                PBNames(i).Location = lname
                PBNames(i).Width = names_width
                PBNames(i).Height = names_height
                PBNames(i).Name = "N" + i.ToString()
                PBNames(i).ForeColor = Color.BurlyWood
                PBNames(i).BackColor = Color.Transparent
                PBNames(i).Text = "Character" + i.ToString()
                Me.Controls.Add(PBNames(i))
                PBNames(i).BringToFront()
                AddHandler PBNames(i).Click, AddressOf Me.ClickNames
                'Portrait of the characters as PictureBox
                PBPortrait(i) = New PictureBox
                PBPortrait(i).Location = lportrait
                PBPortrait(i).Width = portrait_width
                PBPortrait(i).Height = portrait_height
                PBPortrait(i).Name = "P" + (i + 1).ToString()
                PBPortrait(i).SizeMode = PictureBoxSizeMode.StretchImage
                PBPortrait(i).BackColor = Color.Transparent
                Me.Controls.Add(PBPortrait(i))
                PBPortrait(i).BringToFront()
                AddHandler PBPortrait(i).Click, AddressOf Me.ClickPortrait
                '1st Weapon of the characters as PictureBox
                HAND1PIC(i) = New PictureBox
                HAND1PIC(i).Location = lhand1
                HAND1PIC(i).Width = hand1pic_width
                HAND1PIC(i).Height = hand1pic_height
                HAND1PIC(i).Name = "H1-" + i.ToString()
                MyHits1(i) = Now()

                'HAND1PIC(i).Image = BG_DARKPURPLE 'EMPTYINV 'New Bitmap(hand1pic_width, hand1pic_height)
                'HAND1PIC(i).BackgroundImage = BG_DARKPURPLE
                HAND1PIC(i).BackColor = Color.Transparent
                Me.Controls.Add(HAND1PIC(i))
                HAND1PIC(i).BringToFront()
                AddHandler HAND1PIC(i).Click, AddressOf Me.ClickHand1
                AddHandler HAND1PIC(i).MouseHover, AddressOf Me.Hand1Hover
                AddHandler HAND1PIC(i).MouseLeave, AddressOf Me.Hand1Leave

                '2nd Weapon of the characters as PictureBox
                HAND2PIC(i) = New PictureBox
                HAND2PIC(i).Location = lhand2
                HAND2PIC(i).Width = hand1pic_width
                HAND2PIC(i).Height = hand1pic_height
                HAND2PIC(i).Name = "H2-" + i.ToString()
                MyHits2(i) = Now()

                'HAND2PIC(i).Image = BG_DARKPURPLE 'EMPTYINV 'New Bitmap(hand2pic_width, hand2pic_height)
                HAND2PIC(i).BackColor = Color.Transparent
                Me.Controls.Add(HAND2PIC(i))
                HAND2PIC(i).BringToFront()
                AddHandler HAND2PIC(i).Click, AddressOf Me.ClickHand2
                AddHandler HAND2PIC(i).MouseHover, AddressOf Me.Hand2Hover
                AddHandler HAND2PIC(i).MouseLeave, AddressOf Me.Hand2Leave


                'Health of the characters as PictureBox
                HEALTHPIC(i) = New PictureBox
                HEALTHPIC(i).Location = lhealth
                HEALTHPIC(i).Width = healthpic_width
                HEALTHPIC(i).Height = healthpic_height
                HEALTHPIC(i).Name = "H" + i.ToString()
                HEALTHPIC(i).SizeMode = PictureBoxSizeMode.StretchImage
                HEALTHPIC(i).BackColor = Color.DarkSeaGreen
                Me.Controls.Add(HEALTHPIC(i))
                HEALTHPIC(i).BringToFront()
                MANAPIC(i) = New PictureBox
                MANAPIC(i).Location = lmana
                MANAPIC(i).Width = manapic_width
                MANAPIC(i).Height = manapic_height
                MANAPIC(i).Name = "M" + i.ToString()
                MANAPIC(i).SizeMode = PictureBoxSizeMode.StretchImage
                'MANAPIC(i).BackColor = Color.DodgerBlue
                MANAPIC(i).BackColor = Color.RoyalBlue

                'MANAPIC(i).BackColor = Color.CornflowerBlue
                'MANAPIC(i).BackColor = Color.DeepSkyBlue

                'Me.Controls.Add(MANAPIC(i))
                'MANAPIC(i).BringToFront()


                If ((i + 1) Mod 2) = 0 Then
                    mage = False
                    lname.X = names_xpos
                    lname.Y = lname.Y + names_vertical
                    lportrait.X = portrait_xpos
                    lportrait.Y = lportrait.Y + portrait_vertical
                    lhand1.X = hand1pic_xpos
                    lhand1.Y = lhand1.Y + hand1pic_vertical
                    lhand2.X = hand2pic_xpos
                    lhand2.Y = lhand2.Y + hand2pic_vertical
                    lhealth.X = healthpic_xpos
                    lhealth.Y = lhealth.Y + healthpic_vertical
                    lmana.X = manapic_xpos
                    lmana.Y = lmana.Y + manapic_vertical
                Else
                    mage = True
                    lname.X = lname.X + names_horizontal
                    lportrait.X = lportrait.X + portrait_horizontal
                    lhand1.X = lhand1.X + hand1pic_horizontal
                    lhand2.X = lhand2.X + hand2pic_horizontal
                    lhealth.X = lhealth.X + healthpic_horizontal
                    lmana.X = lmana.X + manapic_horizontal

                End If

            Next

            HEALTHMONSTER = New PictureBox
            HEALTHMONSTER.Location = New Point(healthmonster_xpos, healthmonster_ypos)
            HEALTHMONSTER.Width = healthmonster_width
            HEALTHMONSTER.Height = healthmonster_height
            HEALTHMONSTER.Name = "HEALTHMONSTER"
            HEALTHMONSTER.SizeMode = PictureBoxSizeMode.StretchImage
            HEALTHMONSTER.BackColor = Color.DarkSeaGreen
            Me.Controls.Add(HEALTHMONSTER)
            HEALTHMONSTER.BringToFront()


            CampButton = New PictureBox
            CampButton.Location = New Point(CampButton_xpos, CampButton_ypos)
            CampButton.Width = CampButton_width
            CampButton.Height = CampButton_height
            CampButton.Name = "CampButton"
            CampButton.SizeMode = PictureBoxSizeMode.StretchImage
            CampButton.BackColor = Color.Transparent
            Me.Controls.Add(CampButton)
            CampButton.BringToFront()
            AddHandler CampButton.Click, AddressOf Me.ClickCampButton


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "CreateControls")

        End Try



    End Sub

    Private Function CheckCampPlaces(ByVal lnr As Integer) As Integer
        Dim rc As Integer

        Try

            Select Case lnr

                Case 1
                    If (CurrentPosX = 9 And CurrentPosY = 9) _
                    Or (CurrentPosX = 12 And CurrentPosY = 16) _
                    Or (CurrentPosX = 21 And CurrentPosY = 16) _
                    Or (CurrentPosX = 24 And CurrentPosY = 10) Then
                        rc = 1
                    Else
                        rc = 0
                    End If
                Case 2
                    If (CurrentPosX = 12 And CurrentPosY = 16) Then
                        rc = 1
                    Else
                        rc = 0
                    End If

                Case 6
                    If (CurrentPosX = 14 And CurrentPosY = 12) Then
                        rc = 1
                    Else
                        rc = 0
                    End If

            End Select

            Return rc


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "CheckCampPlaces(" + lnr.ToString() + ")")

            Return rc

        End Try


    End Function


    Private Sub ClickCampButton(ByVal sender As System.Object, ByVal e As System.EventArgs)

        'Party wants to rest !!!
        Dim str As String
        Dim ind, ind1, ind2, Hits, rc As Integer

        Try

            rc = CheckCampPlaces(CurrentLevel)
            If rc = 1 Then
                For i = 0 To 3
                    GameChars(i).Health = MaxHealth
                    GameChars(i).ShowHealth()
                Next
                For i = 0 To 5
                    MySpells(i) = MaxSpells
                Next
                MsgBox("Everyone is fully rested and health is restored !")

            Else
                MsgBox("It is not save to rest here ! Monsters are near !")
            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClickCampButton")

        End Try


    End Sub


    Public Sub CheckCurrentPosition()

        'Change View if Monsters are near
        Dim msg As String
        Dim pos, posx, posy As Integer

        posx = CurrentPosX
        posy = CurrentPosY

        Try

            'Monsters in the environment
            If posx > 0 And MyMonsters(posx - 1, posy) > 0 Then
                CurrentDirection = 3
                CurrentView = 3

            ElseIf posx < 31 And MyMonsters(posx + 1, posy) > 0 Then
                CurrentDirection = 1
                CurrentView = 1

            ElseIf posy > 0 And MyMonsters(posx, posy - 1) > 0 Then
                CurrentDirection = 0
                CurrentView = 0
                'Me.ClickRightturn()

            ElseIf posy < 31 And MyMonsters(posx, posy + 1) > 0 Then
                CurrentDirection = 2
                CurrentView = 2

            End If

            'Test for end of Game
            If GameCompleted = False Then
                If CurrentLevel = 2 And posx = 10 And posy = 14 Then
                    msg = "This is the end of Darkmoon RPG The introduction ! " + vbCrLf
                    msg = msg + "From here the story will continue in the next module" + vbCrLf
                    msg = msg + "Darkmoon RPG The Catacombs" + vbCrLf + vbCrLf
                    msg = msg + "Please feel free to continue playing and explore all maps !"
                    GameCompleted = True
                    MsgBox(msg, MsgBoxStyle.Information, "End of the Introduction")
                End If

            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "CheckCurrentPosition")

        End Try


    End Sub


    Public Sub ClickRight()

        Dim pos, posx, posy As Integer

        Try

            posx = CurrentPosX
            posy = CurrentPosY
            If FightGoingOn = True Then
                Return
            Else
                p_monster.Visible = False
            End If

            Select Case CurrentDirection

                Case 0 'north
                    If posx > 30 Then
                        Return
                    End If
                    If MyFields(posx + 1, posy) = 1 Then 'field = empty
                        pos = pos + 1
                        posx = posx + 1
                        CurrentPosX = posx
                        CurrentPosX3 = CurrentPosX3 + 1
                        'CurrentPosition = pos
                        Me.CheckCurrentPosition()
                        Me.PaintWalls()
                        Me.PaintItems()
                    Else
                        Return
                    End If

                Case 1 'east
                    If posy > 30 Then
                        Return
                    End If
                    If MyFields(posx, posy + 1) = 1 Then 'field = empty
                        pos = pos + 32
                        posy = posy + 1
                        CurrentPosY = posy
                        CurrentPosY3 = CurrentPosY3 + 1
                        'CurrentPosition = pos
                        Me.CheckCurrentPosition()
                        Me.PaintWalls()
                        Me.PaintItems()
                    Else
                        Return
                    End If

                Case 2 'south

                    If posx < 1 Then
                        Return
                    End If
                    If MyFields(posx - 1, posy) = 1 Then 'field = empty
                        pos = pos - 1
                        posx = posx - 1
                        CurrentPosX = posx
                        CurrentPosX3 = CurrentPosX3 - 1
                        'CurrentPosition = pos
                        Me.CheckCurrentPosition()
                        Me.PaintWalls()
                        Me.PaintItems()
                    Else
                        Return
                    End If

                Case 3 'west
                    If posy < 1 Then
                        Return
                    End If
                    If MyFields(posx, posy - 1) = 1 Then 'field = empty
                        pos = pos - 32
                        posy = posy - 1
                        CurrentPosY = posy
                        CurrentPosY3 = CurrentPosY3 - 1
                        'CurrentPosition = pos
                        Me.CheckCurrentPosition()
                        Me.PaintWalls()
                        Me.PaintItems()
                    Else
                        Return
                    End If

            End Select


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClickRight")


        End Try


    End Sub

    Public Sub ClickRightturn()

        Try

            If FightGoingOn = True Then
                Return
            Else
                p_monster.Visible = False
            End If

            Select Case CurrentDirection

                Case 0
                    CurrentDirection = 1
                    CurrentView = 1
                    'Me.BackgroundImage = EastBitmap
                    'PBDirection.Image = EastBitmap
                    Me.PaintWalls()
                    Me.PaintItems()

                Case 1
                    CurrentDirection = 2
                    CurrentView = 2
                    'Me.BackgroundImage = SouthBitmap
                    'PBDirection.Image = SouthBitmap
                    Me.PaintWalls()
                    Me.PaintItems()

                Case 2
                    CurrentDirection = 3
                    CurrentView = 3
                    'Me.BackgroundImage = WestBitmap
                    'PBDirection.Image = WestBitmap
                    Me.PaintWalls()
                    Me.PaintItems()

                Case 3
                    CurrentDirection = 0
                    CurrentView = 0
                    'Me.BackgroundImage = NorthBitmap
                    'PBDirection.Image = NorthBitmap
                    Me.PaintWalls()
                    Me.PaintItems()

            End Select


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClickRightturn")

        End Try


    End Sub


    Public Sub ClickLeft()

        Dim pos, posx, posy As Integer

        Try

            If FightGoingOn = True Then
                Return
            Else
                p_monster.Visible = False
            End If

            posx = CurrentPosX
            posy = CurrentPosY
            Select Case CurrentDirection

                Case 0 'north
                    If posx < 1 Then
                        Return
                    End If
                    If MyFields(posx - 1, posy) = 1 Then 'field = empty
                        pos = pos - 1
                        posx = posx - 1
                        CurrentPosX = posx
                        CurrentPosX3 = CurrentPosX3 - 1
                        'CurrentPosition = pos
                        Me.CheckCurrentPosition()
                        Me.PaintWalls()
                        Me.PaintItems()
                    Else
                        Return
                    End If

                Case 1 'east
                    If posy < 1 Then
                        Return
                    End If
                    If MyFields(posx, posy - 1) = 1 Then 'field = empty
                        pos = pos - 32
                        posy = posy - 1
                        CurrentPosY = posy
                        CurrentPosY3 = CurrentPosY3 - 1
                        'CurrentPosition = pos
                        Me.CheckCurrentPosition()
                        Me.PaintWalls()
                        Me.PaintItems()
                    Else
                        Return
                    End If

                Case 2 'south

                    If posx > 30 Then
                        Return
                    End If
                    If MyFields(posx + 1, posy) = 1 Then 'field = empty
                        pos = pos + 1
                        posx = posx + 1
                        CurrentPosX = posx
                        CurrentPosX3 = CurrentPosX3 + 1
                        'CurrentPosition = pos
                        Me.CheckCurrentPosition()
                        Me.PaintWalls()
                        Me.PaintItems()
                    Else
                        Return
                    End If

                Case 3 'west
                    If posy > 30 Then
                        Return
                    End If
                    If MyFields(posx, posy + 1) = 1 Then 'field = empty
                        pos = pos + 32
                        posy = posy + 1
                        CurrentPosY = posy
                        CurrentPosY3 = CurrentPosY3 + 1
                        'CurrentPosition = pos
                        Me.CheckCurrentPosition()
                        Me.PaintWalls()
                        Me.PaintItems()
                    Else
                        Return
                    End If

            End Select

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClickLeft")

        End Try

    End Sub


    Public Sub ClickLeftturn()

        Try

            If FightGoingOn = True Then
                Return
            Else
                p_monster.Visible = False
            End If

            Select Case CurrentDirection

                Case 0
                    CurrentDirection = 3
                    CurrentView = 3
                    'Me.BackgroundImage = WestBitmap
                    'PBDirection.Image = WestBitmap
                    Me.PaintWalls()
                    Me.PaintItems()

                Case 1
                    CurrentDirection = 0
                    CurrentView = 0
                    'Me.BackgroundImage = NorthBitmap
                    'PBDirection.Image = NorthBitmap
                    Me.PaintWalls()
                    Me.PaintItems()

                Case 2
                    CurrentDirection = 1
                    CurrentView = 1
                    'Me.BackgroundImage = EastBitmap
                    'PBDirection.Image = EastBitmap
                    Me.PaintWalls()
                    Me.PaintItems()

                Case 3
                    CurrentDirection = 2
                    CurrentView = 2
                    'Me.BackgroundImage = SouthBitmap
                    'PBDirection.Image = SouthBitmap
                    Me.PaintWalls()
                    Me.PaintItems()

            End Select

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClickLeftturn")

        End Try


    End Sub

    Public Sub ClickDown()

        Dim pos, posx, posy As Integer

        Try

            If FightGoingOn = True Then
                Return
            Else
                p_monster.Visible = False
            End If

            posx = CurrentPosX
            posy = CurrentPosY

            Select Case CurrentDirection
                Case 0 'north
                    If posy > 30 Then
                        Return
                    End If
                    If MyFields(posx, posy + 1) = 1 Then 'field = empty
                        pos = pos + 32
                        'CurrentPosition = pos
                        posy = posy + 1
                        CurrentPosY = posy
                        CurrentPosY3 = CurrentPosY3 + 1
                        Me.CheckCurrentPosition()
                        Me.PaintWalls()
                        Me.PaintItems()
                    Else
                        Return
                    End If

                Case 1 'east
                    If posx > 30 Then
                        Return
                    End If
                    If MyFields(posx - 1, posy) = 1 Then 'field = empty
                        pos = pos - 1
                        posx = posx - 1
                        CurrentPosX = posx
                        'CurrentPosition = pos
                        CurrentPosX3 = CurrentPosX3 - 1
                        Me.CheckCurrentPosition()
                        Me.PaintWalls()
                        Me.PaintItems()
                    Else
                        Return
                    End If

                Case 2 'south
                    If posx < 1 Then
                        Return
                    End If
                    If MyFields(posx, posy - 1) = 1 Then 'field = empty
                        pos = pos - 32
                        'CurrentPosition = pos
                        posy = posy - 1
                        CurrentPosY = posy
                        CurrentPosY3 = CurrentPosY3 - 1
                        Me.CheckCurrentPosition()
                        Me.PaintWalls()
                        Me.PaintItems()
                    Else
                        Return
                    End If


                Case 3 'west
                    If posx > 30 Then
                        Return
                    End If
                    If MyFields(posx + 1, posy) = 1 Then 'field = empty
                        pos = pos + 1
                        'CurrentPosition = pos
                        posx = posx + 1
                        CurrentPosX = posx
                        CurrentPosX3 = CurrentPosX3 + 1
                        Me.CheckCurrentPosition()
                        Me.PaintWalls()
                        Me.PaintItems()
                    Else
                        Return
                    End If
            End Select


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClickDown")

        End Try



    End Sub

    Public Sub ClickUp()

        Dim pos, posx, posy As Integer
        Dim MyBlock As Integer
        Dim NewLevel, NewPosX, NewPosY As Integer

        Try

            If FightGoingOn = True Then
                Return
            Else
                p_monster.Visible = False
            End If

            posx = CurrentPosX
            posy = CurrentPosY

            Select Case CurrentDirection
                Case 0 'north
                    If posy < 1 Then
                        Return
                    End If
                    MyBlock = MyFields(posx, posy - 1)
                    Select Case MyBlock
                        Case 0, 7   ' 8 'solid wall and closed door
                            Return
                        Case 1, 9 'walkable road and opened door
                            posy = posy - 1
                            CurrentPosY = posy
                            CurrentPosY3 = CurrentPosY3 - 1
                            Me.CheckCurrentPosition()
                            Me.PaintWalls()
                            Me.PaintItems()


                        Case 5, 6 'trep down and up
                            Me.GetNewPositions(CurrentLevel, CurrentPosX, CurrentPosY - 1, NewLevel, NewPosX, NewPosY)
                            CurrentLevel = NewLevel
                            CurrentPosX = NewPosX
                            CurrentPosY = NewPosY
                            CurrentPosX3 = CurrentPosX + 3
                            CurrentPosY3 = CurrentPosY + 3
                            BackdropId = Backdrops(CurrentLevel)
                            WallsId = Walls(CurrentLevel)
                            StairsId = Stairs(CurrentLevel)
                            DoorId = Doors(CurrentLevel)

                            Dim bmp1 As Bitmap
                            bmp1 = Me.GetBackdrop(BackdropId)
                            PictureBox1.Image = bmp1 'GameField 'bmp1

                            Me.InitializeBitmaps()
                            Me.FillMapsandItems()
                            Me.ClearCurrentPosition()

                            'Me.FillMapsDB(CurrentLevel)
                            'Me.FillMapsandItems()

                            Me.CheckCurrentPosition()
                            Me.PaintWalls()
                            Me.PaintItems()

                        Case 9 'portal movement
                            'Me.GetPortalPositions(CurrentLevel, CurrentPosX, CurrentPosY - 1, NewLevel, NewPosX, NewPosY)

                            CurrentLevel = NewLevel
                            CurrentPosX = NewPosX
                            CurrentPosY = NewPosY
                            CurrentPosX3 = CurrentPosX + 3
                            CurrentPosY3 = CurrentPosY + 3
                            Me.FillMapsDB(CurrentLevel)
                            Me.CheckCurrentPosition()
                            Me.PaintWalls()
                            Me.PaintItems()

                    End Select


                Case 1 'east
                    If posx > 30 Then
                        Return
                    End If
                    MyBlock = MyFields(posx + 1, posy)

                    Select Case MyBlock
                        Case 0, 7    ', 8 'solid wall
                            Return
                        Case 1, 9 'walkable road
                            posx = posx + 1
                            CurrentPosX = posx
                            CurrentPosX3 = CurrentPosX3 + 1
                            Me.CheckCurrentPosition()
                            Me.PaintWalls()
                            Me.PaintItems()


                        Case 5, 6 'trep down and up
                            Me.GetNewPositions(CurrentLevel, CurrentPosX + 1, CurrentPosY, NewLevel, NewPosX, NewPosY)

                            CurrentLevel = NewLevel
                            CurrentPosX = NewPosX
                            CurrentPosY = NewPosY
                            CurrentPosX3 = CurrentPosX + 3
                            CurrentPosY3 = CurrentPosY + 3

                            BackdropId = Backdrops(CurrentLevel)
                            WallsId = Walls(CurrentLevel)
                            StairsId = Stairs(CurrentLevel)
                            DoorId = Doors(CurrentLevel)


                            Me.InitializeBitmaps()
                            Me.FillMapsandItems()
                            Me.ClearCurrentPosition()
                            Me.CheckCurrentPosition()

                            Me.PaintWalls()
                            Me.PaintItems()


                        Case 9 'portal
                            'Me.GetPortalPositions(CurrentLevel, CurrentPosX + 1, CurrentPosY, NewLevel, NewPosX, NewPosY)

                            CurrentLevel = NewLevel
                            CurrentPosX = NewPosX
                            CurrentPosY = NewPosY
                            CurrentPosX3 = CurrentPosX + 3
                            CurrentPosY3 = CurrentPosY + 3
                            Me.FillMapsDB(CurrentLevel)
                            Me.CheckCurrentPosition()
                            Me.PaintWalls()
                            Me.PaintItems()


                    End Select


                Case 2 'south

                    If posy > 30 Then
                        Return
                    End If
                    MyBlock = MyFields(posx, posy + 1)

                    Select Case MyBlock
                        Case 0, 7   ', 8 'solid wall
                            Return
                        Case 1, 9 'walkable road
                            posy = posy + 1
                            CurrentPosY = posy
                            CurrentPosY3 = CurrentPosY3 + 1
                            Me.CheckCurrentPosition()

                            Me.PaintWalls()
                            Me.PaintItems()


                        Case 5, 6 'trep down and up
                            Me.GetNewPositions(CurrentLevel, CurrentPosX, CurrentPosY + 1, NewLevel, NewPosX, NewPosY)

                            CurrentLevel = NewLevel
                            CurrentPosX = NewPosX
                            CurrentPosY = NewPosY
                            CurrentPosX3 = CurrentPosX + 3
                            CurrentPosY3 = CurrentPosY + 3

                            BackdropId = Backdrops(CurrentLevel)
                            WallsId = Walls(CurrentLevel)
                            StairsId = Stairs(CurrentLevel)
                            DoorId = Doors(CurrentLevel)

                            Me.InitializeBitmaps()
                            Me.FillMapsandItems()
                            Me.ClearCurrentPosition()
                            Me.CheckCurrentPosition()

                            Me.PaintWalls()
                            Me.PaintItems()



                        Case 9 'portal
                            'Me.GetPortalPositions(CurrentLevel, CurrentPosX, CurrentPosY + 1, NewLevel, NewPosX, NewPosY)

                            CurrentLevel = NewLevel
                            CurrentPosX = NewPosX
                            CurrentPosY = NewPosY
                            CurrentPosX3 = CurrentPosX + 3
                            CurrentPosY3 = CurrentPosY + 3
                            Me.FillMapsDB(CurrentLevel)
                            Me.CheckCurrentPosition()

                            Me.PaintWalls()
                            Me.PaintItems()


                    End Select


                Case 3 'west
                    If posx < 1 Then
                        Return
                    End If
                    MyBlock = MyFields(posx - 1, posy)

                    Select Case MyBlock
                        Case 0, 7   ', 8 'solid wall
                            Return
                        Case 1, 9 'walkable road
                            posx = posx - 1
                            CurrentPosX = posx
                            CurrentPosX3 = CurrentPosX3 - 1
                            Me.CheckCurrentPosition()

                            Me.PaintWalls()
                            Me.PaintItems()


                        Case 5, 6 'trep down and up
                            Me.GetNewPositions(CurrentLevel, CurrentPosX - 1, CurrentPosY, NewLevel, NewPosX, NewPosY)

                            CurrentLevel = NewLevel
                            CurrentPosX = NewPosX
                            CurrentPosY = NewPosY
                            CurrentPosX3 = CurrentPosX + 3
                            CurrentPosY3 = CurrentPosY + 3

                            BackdropId = Backdrops(CurrentLevel)
                            WallsId = Walls(CurrentLevel)
                            StairsId = Stairs(CurrentLevel)
                            DoorId = Doors(CurrentLevel)

                            Me.InitializeBitmaps()
                            Me.FillMapsandItems()
                            Me.ClearCurrentPosition()
                            Me.CheckCurrentPosition()

                            Me.PaintWalls()
                            Me.PaintItems()



                        Case 9 'portal
                            'Me.GetPortalPositions(CurrentLevel, CurrentPosX - 1, CurrentPosY, NewLevel, NewPosX, NewPosY)

                            CurrentLevel = NewLevel
                            CurrentPosX = NewPosX
                            CurrentPosY = NewPosY
                            CurrentPosX3 = CurrentPosX + 3
                            CurrentPosY3 = CurrentPosY + 3
                            Me.FillMapsDB(CurrentLevel)
                            Me.CheckCurrentPosition()

                            Me.PaintWalls()
                            Me.PaintItems()


                    End Select
            End Select

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClickUp")

        End Try


    End Sub
 

    Private Sub ClickPortrait(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim str As String
        Dim ind As Integer
        Dim btn As PictureBox

        Try

            btn = CType(sender, PictureBox)

            str = btn.Name
            str = str.Substring(1, 1)
            ind = CInt(str)

            If InvVisible = True Then
                Me.RepositionInventory("right")
                InvVisible = False
                Return
            End If
            CurChar = ind
            Me.RepositionInventory("left")
            PBInvUser.Image = Me.LoadPortrait(GameChars(ind - 1).Picnr) 'GameChars(ind - 1).GetPortrait()
            GameChars(ind - 1).ShowInventory()
            InvVisible = True

            'If ind = CurChar Then 'Character already selected. Do action
            '    Me.RepositionInventory("left")
            '    PBInvUser.Image = Me.LoadPortrait(GameChars(ind - 1).Picnr) 'GameChars(ind - 1).GetPortrait()
            '    GameChars(ind - 1).ShowInventory()

            'Else
            '    CurChar = ind
            '    '    lstSpells1.Visible = True
            '    '    bCast.Visible = True
            '    '    bAbort.Visible = True

            'End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClickPortrait(" + ind.ToString() + ")")

        End Try

    End Sub


    Private Sub ClickNames(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim str As String
        Dim ind As Integer
        Dim btn As Label

        Try

            btn = CType(sender, Label)
            str = btn.Name
            str = str.Substring(1, 1)
            ind = CInt(str)

            'MsgBox("you clicked button: " + str)

            If FlipOperation = False Then
                btn.Font = New Font(btn.Font, FontStyle.Bold)
                Flipsource = ind + 1
                FlipOperation = True
            Else
                Fliptarget = ind + 1
                If Flipsource <> Fliptarget Then
                    Me.FlipControlsNew(Flipsource, Fliptarget)
                Else
                End If
                PBNames(ind).Font() = New Font(PBNames(ind).Font, FontStyle.Regular)
                Flipsource = 0
                FlipOperation = False
            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClickNames(" + ind.ToString() + ")")

        End Try

    End Sub

    Public Sub NewCursor(ByVal bmp As Bitmap, ByVal w As Integer, ByVal h As Integer)

        Try

            Dim img As New Bitmap(w, h)
            img = bmp
            img.MakeTransparent(Color.Black)
            Me.Cursor = create.CreateCursor(img, w, h)

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "NewCursor")

        End Try

    End Sub

    Public Sub PositionControls()

        Try

            P001.Location = New Point(HEALTHPIC(0).Location.X, HEALTHPIC(0).Location.Y - 1)
            P001.Width = HEALTHPIC(0).Width
            P001.Height = HEALTHPIC(0).Height + 2
            P002.Location = New Point(HEALTHPIC(1).Location.X, HEALTHPIC(1).Location.Y - 1)
            P002.Width = HEALTHPIC(0).Width
            P002.Height = HEALTHPIC(0).Height + 2
            P003.Location = New Point(HEALTHPIC(2).Location.X, HEALTHPIC(2).Location.Y - 1)
            P003.Width = HEALTHPIC(0).Width
            P003.Height = HEALTHPIC(0).Height + 2
            P004.Location = New Point(HEALTHPIC(3).Location.X, HEALTHPIC(3).Location.Y - 1)
            P004.Width = HEALTHPIC(0).Width
            P004.Height = HEALTHPIC(0).Height + 2
            p_health2.Location = New Point(HEALTHMONSTER.Location.X, HEALTHMONSTER.Location.Y - 2)
            p_health2.Width = HEALTHMONSTER.Width
            p_health2.Height = HEALTHMONSTER.Height + 3
            'p_health2.BringToFront()

            p_monster.Location = New Point(PictureBox1.Location.X, PictureBox1.Location.Y)
            p_monster.Width = PictureBox1.Width
            p_monster.Height = PictureBox1.Height
            p_effects.Location = New Point(PictureBox1.Location.X, PictureBox1.Location.Y)
            p_effects2.Location = New Point(PictureBox1.Location.X, PictureBox1.Location.Y)
            p_effects3.Location = New Point(PictureBox1.Location.X, PictureBox1.Location.Y)

            p_effects.Width = PictureBox1.Width
            p_effects.Height = PictureBox1.Height
            p_effects2.Width = PictureBox1.Width
            p_effects2.Height = PictureBox1.Height
            p_effects3.Width = PictureBox1.Width
            p_effects3.Height = PictureBox1.Height


            Dim bmp As Bitmap
            bmp = p_effects.Image
            bmp.MakeTransparent(Color.Black)
            p_effects.Image = bmp

            bmp = p_effects2.Image
            bmp.MakeTransparent(Color.Black)
            p_effects2.Image = bmp

            bmp = p_effects3.Image
            bmp.MakeTransparent(Color.Black)
            p_effects3.Image = bmp

            p_health2.Visible = False
            HEALTHMONSTER.Visible = False

            RichTextBox1.Location = New Point(Text_xpos, Text_ypos)
            RichTextBox1.Width = Text_width
            RichTextBox1.Height = Text_height

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PositionControls")

        End Try



    End Sub

    Public Sub LoadDatasets()

        Try
            Me.LoadTreps()
            Me.LoadPortals()
            Me.LoadDoors()
            Me.LoadItemtypes()
            Me.LoadItemsFloor()
            Me.LoadItemsWall()
            Me.LoadGameField(GameFieldId)
            Me.LoadWalltypes(WalltypeId)
            Me.LoadMonsters()


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadDatasets")

        End Try

    End Sub

    Public Sub LoadWalltypes(ByVal wtid As Integer)

        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection

        Try

            dsWallTypes.Reset()
            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            da = New OleDbDataAdapter
            mysql = "select * from Walltypes where walltypeid = " + wtid.ToString()
            da.SelectCommand = New OleDbCommand(mysql, myconnection1)

            myconnection1.Open()
            da.Fill(dsWallTypes, "Walltypes")

            myconnection1.Close()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadWalltypes(" + wtid.ToString() + ")")

        End Try

    End Sub

    Public Sub LoadGameField(ByVal gf As Integer)

        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection

        Try

            dsGameField.Reset()
            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            da = New OleDbDataAdapter
            mysql = "select * from Gamefield where fieldnr = " + gf.ToString()
            da.SelectCommand = New OleDbCommand(mysql, myconnection1)

            myconnection1.Open()
            da.Fill(dsGameField, "Gamefield")

            myconnection1.Close()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadGameField(" + gf.ToString() + ")")

        End Try

    End Sub

    Public Sub LoadMonsters()

        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection

        Try

            dsMonsters.Reset()
            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            da = New OleDbDataAdapter
            mysql = "select * from Monsters"
            da.SelectCommand = New OleDbCommand(mysql, myconnection1)

            myconnection1.Open()
            da.Fill(dsMonsters, "Monsters")

            myconnection1.Close()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadMonsters")

        End Try

    End Sub

    Public Sub LoadItemsWall()

        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection

        Try

            dsItemsWall.Reset()
            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            da = New OleDbDataAdapter
            mysql = "select * from Wallitems"
            da.SelectCommand = New OleDbCommand(mysql, myconnection1)

            myconnection1.Open()
            da.Fill(dsItemsWall, "Wallitems")

            myconnection1.Close()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadItemsWall")

        End Try



    End Sub

    Public Sub LoadItemsFloor()

        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection

        Try

            dsItemsFloor.Reset()
            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            da = New OleDbDataAdapter
            mysql = "select * from Mapitems"
            da.SelectCommand = New OleDbCommand(mysql, myconnection1)

            myconnection1.Open()
            da.Fill(dsItemsFloor, "Mapitems")

            myconnection1.Close()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadItemsFloor")

        End Try


    End Sub

    Public Sub LoadItemtypes()

        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection

        Try

            dsItemtypes.Reset()
            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            da = New OleDbDataAdapter
            mysql = "select * from Itemtypes"
            da.SelectCommand = New OleDbCommand(mysql, myconnection1)

            myconnection1.Open()
            da.Fill(dsItemtypes, "Itemtypes")

            myconnection1.Close()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadItemtypes")

        End Try

    End Sub


    Public Sub LoadDoors()

        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection

        Try

            dsDoors.Reset()
            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            da = New OleDbDataAdapter
            mysql = "select * from Doors"
            da.SelectCommand = New OleDbCommand(mysql, myconnection1)

            myconnection1.Open()
            da.Fill(dsDoors, "Doors")

            myconnection1.Close()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadDoors")

        End Try

    End Sub

    Public Sub LoadPortals()
        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection

        Try

            dsPortals.Reset()
            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            da = New OleDbDataAdapter
            mysql = "select * from Portals"
            da.SelectCommand = New OleDbCommand(mysql, myconnection1)

            myconnection1.Open()
            da.Fill(dsPortals, "Portals")

            myconnection1.Close()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadPortals")

        End Try

    End Sub

    Public Sub LoadTreps()
        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection

        Try

            dsTreps.Reset()
            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            da = New OleDbDataAdapter
            mysql = "select * from Stairs"
            da.SelectCommand = New OleDbCommand(mysql, myconnection1)

            myconnection1.Open()

            da.Fill(dsTreps, "Stairs")

            myconnection1.Close()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadTreps")

        End Try

    End Sub

    Public Sub GetDoors()

        Dim mysql, mydoor As String
        Dim mystring As String
        Dim myconnection1 As DbConnection
        Dim mycommand As DbCommand

        Try

            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            mysql = "select pictname from Doortypes where doornr = " + DoorId.ToString()

            mycommand = New OleDbCommand
            mycommand.CommandText = mysql
            mycommand.Connection = myconnection1
            myconnection1.Open()
            mydoor = mycommand.ExecuteScalar()

            myconnection1.Close()


            DOOR01A = Bitmap.FromFile(GameFile + "\doors\" + mydoor + "A-L01.bmp")
            DOOR01A.Tag = mydoor + "A-L01.bmp"
            DOOR02A = Bitmap.FromFile(GameFile + "\doors\" + mydoor + "A-L02.bmp")
            DOOR02A.Tag = mydoor + "A-L02.bmp"
            DOOR03A = Bitmap.FromFile(GameFile + "\doors\" + mydoor + "A-L03.bmp")
            DOOR03A.Tag = mydoor + "A-L03.bmp"
            DOOR01B = Bitmap.FromFile(GameFile + "\doors\" + mydoor + "B-L01.bmp")
            DOOR01B.Tag = mydoor + "B-L01.bmp"
            DOOR02B = Bitmap.FromFile(GameFile + "\doors\" + mydoor + "B-L02.bmp")
            DOOR02B.Tag = mydoor + "B-L02.bmp"
            DOOR03B = Bitmap.FromFile(GameFile + "\doors\" + mydoor + "B-L03.bmp")
            DOOR03B.Tag = mydoor + "B-L03.bmp"


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "GetDoors")

        End Try

    End Sub

    Public Sub GetStairs()

        Dim mysql, mystairs As String
        Dim mystring As String
        Dim myconnection1 As DbConnection
        Dim mycommand As DbCommand

        Try

            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            mysql = "select pictname from Stairstypes where stairsnr = " + StairsId.ToString()

            mycommand = New OleDbCommand
            mycommand.CommandText = mysql
            mycommand.Connection = myconnection1
            myconnection1.Open()
            mystairs = mycommand.ExecuteScalar()

            myconnection1.Close()


            STAIRS01A = Bitmap.FromFile(GameFile + "\stairs\" + mystairs + "A-L01.bmp")
            STAIRS01A.Tag = mystairs + "A-L01.bmp"
            STAIRS02A = Bitmap.FromFile(GameFile + "\stairs\" + mystairs + "A-L02.bmp")
            STAIRS02A.Tag = mystairs + "A-L02.bmp"
            STAIRS03A = Bitmap.FromFile(GameFile + "\stairs\" + mystairs + "A-L03.bmp")
            STAIRS03A.Tag = mystairs + "A-L03.bmp"

            STAIRS01B = Bitmap.FromFile(GameFile + "\stairs\" + mystairs + "B-L01.bmp")
            STAIRS01B.Tag = mystairs + "B-L01.bmp"
            STAIRS02B = Bitmap.FromFile(GameFile + "\stairs\" + mystairs + "B-L02.bmp")
            STAIRS02B.Tag = mystairs + "B-L02.bmp"
            STAIRS03B = Bitmap.FromFile(GameFile + "\stairs\" + mystairs + "B-L03.bmp")
            STAIRS03B.Tag = mystairs + "B-L03.bmp"

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "GetStairs")

        End Try

    End Sub


    Public Sub GetWallBitmaps()

        Dim mysql, mywallname As String
        Dim mystring As String
        Dim myconnection1 As DbConnection
        Dim mycommand As DbCommand

        Try

            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            mysql = "select pictname from Walls where wallnr = " + WallsId.ToString()

            mycommand = New OleDbCommand
            mycommand.CommandText = mysql
            mycommand.Connection = myconnection1
            myconnection1.Open()
            mywallname = mycommand.ExecuteScalar()

            myconnection1.Close()

            'Main Walls
            WALL_L01 = Bitmap.FromFile(GameFile + "\walls\" + mywallname + "-L01.bmp")
            WALL_L01.Tag = mywallname + "-L01.bmp"
            WALL_L02 = Bitmap.FromFile(GameFile + "\walls\" + mywallname + "-L02.bmp")
            WALL_L02.Tag = mywallname + "-L02.bmp"
            WALL_L03 = Bitmap.FromFile(GameFile + "\walls\" + mywallname + "-L03.bmp")
            WALL_L03.Tag = mywallname + "-L03.bmp"

            'Side Walls
            SWLEFT_L01 = Bitmap.FromFile(GameFile + "\walls\" + mywallname + "L-L01.png")
            SWLEFT_L01.Tag = mywallname + "L-L01.png"
            SWLEFT_L02 = Bitmap.FromFile(GameFile + "\walls\" + mywallname + "L-L02.png")
            SWLEFT_L02.Tag = mywallname + "L-L02.png"
            SWLEFT_L03 = Bitmap.FromFile(GameFile + "\walls\" + mywallname + "L-L03.png")
            SWLEFT_L03.Tag = mywallname + "L-L03.png"
            SWLEFT_L04 = Bitmap.FromFile(GameFile + "\walls\" + mywallname + "L-L04.png")
            SWLEFT_L04.Tag = mywallname + "L-L04.png"
            SWRIGHT_L01 = Bitmap.FromFile(GameFile + "\walls\" + mywallname + "R-L01.png")
            SWRIGHT_L01.Tag = mywallname + "R-L01.png"
            SWRIGHT_L02 = Bitmap.FromFile(GameFile + "\walls\" + mywallname + "R-L02.png")
            SWRIGHT_L02.Tag = mywallname + "R-L02.png"
            SWRIGHT_L03 = Bitmap.FromFile(GameFile + "\walls\" + mywallname + "R-L03.png")
            SWRIGHT_L03.Tag = mywallname + "R-L03.png"
            SWRIGHT_L04 = Bitmap.FromFile(GameFile + "\walls\" + mywallname + "R-L04.png")
            SWRIGHT_L04.Tag = mywallname + "R-L04.png"


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "GetWallBitmaps")

        End Try

    End Sub

    Private Sub InitializeBitmaps()

        Try

            'Load the Bitmaps
            'World Backdrop
            GameField = Me.GetBackdrop(BackdropId)

            'Main Walls + SideWalls
            Me.GetWallBitmaps()

            'Stairs Up and Down
            Me.GetStairs()

            'Doors
            Me.GetDoors()

            TELEPORT = Bitmap.FromFile(GameFile + "\portal\T001.gif")

            MISS = Bitmap.FromFile(GameFile + "\health\MISS.bmp")
            HIT = Bitmap.FromFile(GameFile + "\health\HIT02.bmp")
            WOUND = Bitmap.FromFile(GameFile + "\health\WOUND02.bmp")
            HANDLEFT = Bitmap.FromFile(GameFile + "\weapons\handleftT.gif")
            HANDRIGHT = Bitmap.FromFile(GameFile + "\weapons\handrightT.gif")

            EMPTY01 = Bitmap.FromFile(GameFile + "\misc\bg-empty.bmp")
            EMPTYINV = Bitmap.FromFile(GameFile + "\picts\empty-inv.bmp")
            BG_DARKPURPLE = Bitmap.FromFile(GameFile + "\picts\bg-darkpurple.bmp")
            BG_LIGHTPURPLE = Bitmap.FromFile(GameFile + "\picts\bg-lightpurple.bmp")
            SMALL_MAP = Bitmap.FromFile(GameFile + "\picts\bg-map.bmp")

            North = Bitmap.FromFile(GameFile + "\picts\letterN.bmp")
            East = Bitmap.FromFile(GameFile + "\picts\letterE.bmp")
            South = Bitmap.FromFile(GameFile + "\picts\letterS.bmp")
            West = Bitmap.FromFile(GameFile + "\picts\letterW.bmp")
            PictureBox2.Image = Bitmap.FromFile(GameFile + "\picts\bg-map.bmp")


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "InitializeBitmaps")

        End Try

    End Sub


    Public Sub PaintWalls()
        Dim bmp1, bmp2 As Bitmap

        Try

            Select Case CurrentView

                Case 0 'North
                    Me.PaintWallsNorth()
                    bmp1 = North

                Case 1 'East
                    Me.PaintWallsEast()
                    bmp1 = East

                Case 2 'South
                    Me.PaintWallsSouth()
                    bmp1 = South

                Case 3 'West
                    Me.PaintWallsWest()
                    bmp1 = West

            End Select
            bmp2 = PictureBox1.Image
            bmp2 = Me.AddSpriteTransBlack(bmp2, bmp1, 170, 0)
            Me.WriteCurrentPosition()

            'Me.ShowCounters()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PaintWalls")

        End Try

    End Sub

    Public Sub PaintWallsNorth()
        Dim posx, posy As Integer

        Try

            posx = CurrentPosX3
            posy = CurrentPosY3

            p00 = MyFieldsB(posx, posy)
            p10 = MyFieldsB(posx - 1, posy)
            p01 = MyFieldsB(posx + 1, posy)

            p11 = MyFieldsB(posx - 1, posy - 1)
            p12 = MyFieldsB(posx, posy - 1)
            p13 = MyFieldsB(posx + 1, posy - 1)

            p21 = MyFieldsB(posx - 1, posy - 2)
            p22 = MyFieldsB(posx, posy - 2)
            p23 = MyFieldsB(posx + 1, posy - 2)

            p30 = MyFieldsB(posx - 2, posy - 3)
            p31 = MyFieldsB(posx - 1, posy - 3)
            p32 = MyFieldsB(posx, posy - 3)
            p33 = MyFieldsB(posx + 1, posy - 3)
            p34 = MyFieldsB(posx + 2, posy - 3)

            Me.PaintTheWalls(posx, posy, "North")

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PaintWallsNorth")

        End Try

    End Sub


    Public Sub PaintWallsWest()

        Dim posx, posy As Integer
        Dim mainbmp As New Bitmap(355, 240)

        Try

            mainbmp = GameField
            'Bitmap.FromFile(GameFile + "\picts\EOB-BACKDROP-1.bmp")
            posx = CurrentPosX3
            posy = CurrentPosY3
            p10 = MyFieldsB(posx, posy + 1)
            p00 = MyFieldsB(posx, posy)
            p01 = MyFieldsB(posx, posy - 1)
            p11 = MyFieldsB(posx - 1, posy + 1)
            p12 = MyFieldsB(posx - 1, posy)
            p13 = MyFieldsB(posx - 1, posy - 1)
            p21 = MyFieldsB(posx - 2, posy + 1)
            p22 = MyFieldsB(posx - 2, posy)
            p23 = MyFieldsB(posx - 2, posy - 1)
            p30 = MyFieldsB(posx - 3, posy + 2)
            p31 = MyFieldsB(posx - 3, posy + 1)
            p32 = MyFieldsB(posx - 3, posy)
            p33 = MyFieldsB(posx - 3, posy - 1)
            p34 = MyFieldsB(posx - 3, posy - 2)

            Me.PaintTheWalls(posx, posy, "West")

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PaintWallsWest")

        End Try

    End Sub


    Public Sub PaintWallsEast()
        Dim posx, posy As Integer
        Dim mainbmp As New Bitmap(355, 240)

        Try

            mainbmp = GameField
            'Bitmap.FromFile(GameFile + "\picts\EOB-BACKDROP-1.bmp")
            posx = CurrentPosX3
            posy = CurrentPosY3
            p00 = MyFieldsB(posx, posy)
            p10 = MyFieldsB(posx, posy - 1)
            p01 = MyFieldsB(posx, posy + 1)
            p11 = MyFieldsB(posx + 1, posy - 1)
            p12 = MyFieldsB(posx + 1, posy)
            p13 = MyFieldsB(posx + 1, posy + 1)
            p21 = MyFieldsB(posx + 2, posy - 1)
            p22 = MyFieldsB(posx + 2, posy)
            p23 = MyFieldsB(posx + 2, posy + 1)
            p30 = MyFieldsB(posx + 3, posy - 2)
            p31 = MyFieldsB(posx + 3, posy - 1)
            p32 = MyFieldsB(posx + 3, posy)
            p33 = MyFieldsB(posx + 3, posy + 1)
            p34 = MyFieldsB(posx + 3, posy + 2)

            Me.PaintTheWalls(posx, posy, "East")

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PaintWallsEast")

        End Try

    End Sub

    Public Sub PaintWallsSouth()
        Dim posx, posy As Integer
        Dim mainbmp As New Bitmap(355, 240)

        Try

            mainbmp = GameField
            'Bitmap.FromFile(GameFile + "\picts\EOB-BACKDROP-1.bmp")
            posx = CurrentPosX3
            posy = CurrentPosY3
            p00 = MyFieldsB(posx, posy)
            p10 = MyFieldsB(posx + 1, posy)
            p01 = MyFieldsB(posx - 1, posy)
            p11 = MyFieldsB(posx + 1, posy + 1)
            p12 = MyFieldsB(posx, posy + 1)
            p13 = MyFieldsB(posx - 1, posy + 1)
            p21 = MyFieldsB(posx + 1, posy + 2)
            p22 = MyFieldsB(posx, posy + 2)
            p23 = MyFieldsB(posx - 1, posy + 2)
            p30 = MyFieldsB(posx + 2, posy + 3)
            p31 = MyFieldsB(posx + 1, posy + 3)
            p32 = MyFieldsB(posx, posy + 3)
            p33 = MyFieldsB(posx - 1, posy + 3)
            p34 = MyFieldsB(posx - 2, posy + 3)

            Me.PaintTheWalls(posx, posy, "South")

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PaintWallsSouth")

        End Try

    End Sub

    Public Sub PaintTheWalls(ByVal px As Integer, ByVal py As Integer, ByVal dir As String)

        Dim mainbmp As New Bitmap(MainWindow_width, MainWindow_height)
        Dim mainbmp2 As New Bitmap(MainWindow_width, MainWindow_height)
        Dim mainbmpbig As New Bitmap(MainWindow_width * 3, MainWindow_height)


        Try

            'mainbmp = GameField
            mainbmp = Me.GetBackdrop(BackdropId)
            ' Bitmap.FromFile(GameFile + "\picts\EOB-BACKDROP-1.bmp")
            Me.CopyToBigGameMap(mainbmp, mainbmpbig)

            'check position p12 (in front)
            If p12 <> 1 Then
                Me.PaintItemL1(mainbmpbig, p12, 2, MW_lvl1_xpos, MW_lvl1_ypos)
                If p10 <> 1 Then
                    If p10 = 0 Then 'wall
                        Me.PaintItemL1(mainbmpbig, p10, 5, SWL_lvl1_xpos, SWL_lvl1_ypos)
                    ElseIf p10 = 9 Or p10 = 7 Or p10 = 6 Or p10 = 5 Then 'door
                        Me.PaintItemL1(mainbmpbig, p10, 5, MW_lvl1_xpos - MW_lvl1_width, MW_lvl1_ypos)
                    End If
                Else
                    If p11 <> 1 Then 'draw second lvl1 wall left
                        Me.PaintItemL1(mainbmpbig, p11, 1, MW_lvl1_xpos - MW_lvl1_width, MW_lvl1_ypos)
                    End If
                End If
                If p01 <> 1 Then
                    If p01 = 0 Then 'wall
                        Me.PaintItemL1(mainbmpbig, p01, 6, SWR_lvl1_xpos, SWR_lvl1_ypos)
                    ElseIf p01 = 9 Or p01 = 7 Or p01 = 6 Or p01 = 5 Then 'door
                        Me.PaintItemL1(mainbmpbig, p01, 6, MW_lvl1_xpos + MW_lvl1_width, MW_lvl1_ypos)
                    End If
                Else
                    If p13 <> 1 Then 'draw second lvl1 wall left
                        Me.PaintItemL1(mainbmpbig, p13, 3, MW_lvl1_xpos + MW_lvl1_width, MW_lvl1_ypos)
                    End If
                End If
            Else
                'check position p22 (2-nd row)
                If p22 <> 1 Then
                    Me.PaintItemL2(mainbmpbig, p22, 2, MW_lvl2_xpos, MW_lvl2_ypos)
                    If p11 <> 1 Then
                        If p11 = 0 Then
                            Me.PaintItemL1(mainbmpbig, p11, 0, SWL_lvl2_xpos, SWL_lvl2_ypos)
                        ElseIf p11 = 9 Or p11 = 7 Or p11 = 6 Or p11 = 5 Then
                            Me.PaintItemL1(mainbmpbig, p11, 0, MW_lvl2_xpos - MW_lvl2_width, MW_lvl2_ypos)
                        End If
                    Else
                        If p21 <> 1 Then
                            Me.PaintItemL2(mainbmpbig, p21, 1, MW_lvl2_xpos - MW_lvl2_width, MW_lvl2_ypos)
                        End If
                    End If
                    If p13 <> 1 Then
                        If p13 = 0 Then
                            Me.PaintItemL1(mainbmpbig, p13, 4, SWR_lvl2_xpos, SWR_lvl2_ypos)
                        ElseIf p13 = 9 Or p13 = 7 Or p13 = 6 Or p13 = 5 Then
                            Me.PaintItemL1(mainbmpbig, p13, 4, MW_lvl2_xpos + MW_lvl2_width, MW_lvl2_ypos)
                        End If
                    Else
                        If p23 <> 1 Then
                            Me.PaintItemL2(mainbmpbig, p23, 3, MW_lvl2_xpos + MW_lvl2_width, MW_lvl2_ypos)
                        End If
                    End If
                    If p10 <> 1 Then
                        If p10 = 0 Then 'wall
                            Me.PaintItemL1(mainbmpbig, p10, 5, SWL_lvl1_xpos, SWL_lvl1_ypos)
                        ElseIf p10 = 9 Or p10 = 7 Or p10 = 6 Or p10 = 5 Then 'door
                            Me.PaintItemL1(mainbmpbig, p10, 5, MW_lvl1_xpos - MW_lvl1_width, MW_lvl1_ypos)
                        End If
                    Else
                        If p11 <> 1 Then 'draw second lvl1 wall left
                            Me.PaintItemL1(mainbmpbig, p11, 1, MW_lvl1_xpos - MW_lvl1_width, MW_lvl1_ypos)
                        End If
                    End If
                    If p01 <> 1 Then
                        If p01 = 0 Then 'wall
                            Me.PaintItemL1(mainbmpbig, p01, 6, SWR_lvl1_xpos, SWR_lvl1_ypos)
                        ElseIf p01 = 9 Or p01 = 7 Or p01 = 6 Or p01 = 5 Then 'door
                            Me.PaintItemL1(mainbmpbig, p01, 6, MW_lvl1_xpos + MW_lvl1_width, MW_lvl1_ypos)
                        End If
                    Else
                        If p13 <> 1 Then 'draw second lvl1 wall left
                            Me.PaintItemL1(mainbmpbig, p13, 3, MW_lvl1_xpos + MW_lvl1_width, MW_lvl1_ypos)
                        End If
                    End If
                Else
                    'check position p32 (3-nd row)
                    If p32 <> 1 Then
                        Me.PaintItemL3(mainbmpbig, p32, 2, MW_lvl3_xpos, MW_lvl3_ypos)
                        If p21 <> 1 Then
                            If p21 = 0 Then
                                Me.PaintItemL2(mainbmpbig, p21, 0, SWL_lvl3_xpos, SWL_lvl3_ypos)
                                Me.PaintItemL2(mainbmpbig, p21, 1, MW_lvl2_xpos - MW_lvl2_width, MW_lvl2_ypos)
                            ElseIf p21 = 9 Or p21 = 7 Or p21 = 6 Or p21 = 5 Then
                                Me.PaintItemL2(mainbmpbig, p21, 0, MW_lvl3_xpos - MW_lvl3_width, MW_lvl3_ypos)
                            End If

                        Else
                            If p30 <> 1 Then
                                Me.PaintItemL3(mainbmpbig, p30, 0, MW_lvl3_xpos - MW_lvl3_width - MW_lvl3_width, MW_lvl3_ypos)
                            End If
                            If p31 <> 1 Then
                                Me.PaintItemL3(mainbmpbig, p31, 1, MW_lvl3_xpos - MW_lvl3_width, MW_lvl3_ypos)
                            End If

                        End If
                        If p23 <> 1 Then
                            If p23 = 0 Then
                                Me.PaintItemL2(mainbmpbig, p23, 4, SWR_lvl3_xpos, SWR_lvl3_ypos)
                                Me.PaintItemL2(mainbmpbig, p23, 3, MW_lvl2_xpos + MW_lvl2_width, MW_lvl2_ypos)
                            ElseIf p23 = 9 Or p23 = 7 Or p23 = 6 Or p23 = 5 Then
                                Me.PaintItemL2(mainbmpbig, p23, 0, MW_lvl3_xpos + MW_lvl3_width, MW_lvl3_ypos)
                            End If
                        Else
                            If p34 <> 1 Then
                                Me.PaintItemL3(mainbmpbig, p34, 4, MW_lvl3_xpos + MW_lvl3_width + MW_lvl3_width, MW_lvl3_ypos)
                            End If
                            If p33 <> 1 Then
                                Me.PaintItemL3(mainbmpbig, p33, 3, MW_lvl3_xpos + MW_lvl3_width, MW_lvl3_ypos)
                            End If

                        End If
                        If p11 <> 1 Then
                            If p11 = 0 Then
                                Me.PaintItemL1(mainbmpbig, p11, 0, SWL_lvl2_xpos, SWL_lvl2_ypos)
                            ElseIf p11 = 9 Or p11 = 7 Or p11 = 6 Or p11 = 5 Then
                                Me.PaintItemL1(mainbmpbig, p11, 0, MW_lvl2_xpos - MW_lvl2_width, MW_lvl2_ypos)
                            End If
                        End If
                        If p13 <> 1 Then
                            If p13 = 0 Then
                                Me.PaintItemL1(mainbmpbig, p13, 4, SWR_lvl2_xpos, SWR_lvl2_ypos)
                            ElseIf p13 = 9 Or p13 = 7 Or p13 = 6 Or p13 = 5 Then
                                Me.PaintItemL1(mainbmpbig, p13, 4, MW_lvl2_xpos + MW_lvl2_width, MW_lvl2_ypos)
                            End If
                        End If
                        If p10 <> 1 Then
                            If p10 = 0 Then 'wall
                                Me.PaintItemL1(mainbmpbig, p10, 5, SWL_lvl1_xpos, SWL_lvl1_ypos)
                            ElseIf p10 = 9 Or p10 = 7 Or p10 = 6 Or p10 = 5 Then 'door
                                Me.PaintItemL1(mainbmpbig, p10, 5, MW_lvl1_xpos - MW_lvl1_width, MW_lvl1_ypos)
                            End If
                        Else
                            If p11 <> 1 Then 'draw second lvl1 wall left
                                Me.PaintItemL1(mainbmpbig, p11, 1, MW_lvl1_xpos - MW_lvl1_width, MW_lvl1_ypos)
                            End If
                        End If
                        If p01 <> 1 Then
                            If p01 = 0 Then 'wall
                                Me.PaintItemL1(mainbmpbig, p01, 6, SWR_lvl1_xpos, SWR_lvl1_ypos)
                            ElseIf p01 = 9 Or p01 = 7 Or p01 = 6 Or p01 = 5 Then 'door
                                Me.PaintItemL1(mainbmpbig, p01, 6, MW_lvl1_xpos + MW_lvl1_width, MW_lvl1_ypos)
                            End If
                        Else
                            If p13 <> 1 Then 'draw second lvl1 wall left
                                Me.PaintItemL1(mainbmpbig, p13, 3, MW_lvl1_xpos + MW_lvl1_width, MW_lvl1_ypos)
                            End If
                        End If
                    Else
                        If p30 <> 1 Then
                            Me.PaintItemL3(mainbmpbig, p31, 1, MW_lvl3_xpos - MW_lvl3_width - MW_lvl3_width, MW_lvl3_ypos)
                        End If
                        If p31 <> 1 Then
                            Me.PaintItemL3(mainbmpbig, p31, 1, MW_lvl3_xpos - MW_lvl3_width, MW_lvl3_ypos)
                        End If
                        If p33 <> 1 Then
                            Me.PaintItemL3(mainbmpbig, p33, 3, MW_lvl3_xpos + MW_lvl3_width, MW_lvl3_ypos)
                        End If
                        If p34 <> 1 Then
                            Me.PaintItemL3(mainbmpbig, p33, 3, MW_lvl3_xpos + MW_lvl3_width + MW_lvl3_width, MW_lvl3_ypos)
                        End If
                        If p21 <> 1 Then
                            If p21 = 0 Then
                                Me.PaintItemL2(mainbmpbig, p21, 0, SWL_lvl3_xpos, SWL_lvl3_ypos)
                                Me.PaintItemL2(mainbmpbig, p21, 1, MW_lvl2_xpos - MW_lvl2_width, MW_lvl2_ypos)
                            ElseIf p21 = 9 Or p21 = 7 Or p21 = 6 Or p21 = 5 Then
                                Me.PaintItemL2(mainbmpbig, p21, 0, MW_lvl3_xpos - MW_lvl3_width, MW_lvl3_ypos)
                            End If
                        End If
                        If p23 <> 1 Then
                            If p23 = 0 Then
                                Me.PaintItemL2(mainbmpbig, p23, 4, SWR_lvl3_xpos, SWR_lvl3_ypos)
                                Me.PaintItemL2(mainbmpbig, p23, 3, MW_lvl2_xpos + MW_lvl2_width, MW_lvl2_ypos)
                            ElseIf p23 = 9 Or p23 = 7 Or p23 = 6 Or p23 = 5 Then
                                Me.PaintItemL2(mainbmpbig, p23, 0, MW_lvl3_xpos + MW_lvl3_width, MW_lvl3_ypos)
                            End If
                        End If
                        If p11 <> 1 Then
                            If p11 = 0 Then
                                Me.PaintItemL1(mainbmpbig, p11, 0, SWL_lvl2_xpos, SWL_lvl2_ypos)
                            ElseIf p11 = 9 Or p11 = 7 Or p11 = 6 Or p11 = 5 Then
                                Me.PaintItemL1(mainbmpbig, p11, 0, MW_lvl2_xpos - MW_lvl2_width, MW_lvl2_ypos)
                            End If
                        End If
                        If p13 <> 1 Then
                            If p13 = 0 Then
                                Me.PaintItemL1(mainbmpbig, p13, 4, SWR_lvl2_xpos, SWR_lvl2_ypos)
                            ElseIf p13 = 9 Or p13 = 7 Or p13 = 6 Or p13 = 5 Then
                                Me.PaintItemL1(mainbmpbig, p13, 4, MW_lvl2_xpos + MW_lvl2_width, MW_lvl2_ypos)
                            End If
                        End If
                        If p10 <> 1 Then
                            'Me.PaintItemL1(mainbmpbig, p10, 5, SWL_lvl1_xpos, SWL_lvl1_ypos)
                            If p10 = 0 Then 'wall
                                Me.PaintItemL1(mainbmpbig, p10, 5, SWL_lvl1_xpos, SWL_lvl1_ypos)
                            ElseIf p10 = 9 Or p10 = 7 Or p10 = 6 Or p10 = 5 Then 'door
                                Me.PaintItemL1(mainbmpbig, p10, 5, MW_lvl1_xpos - MW_lvl1_width, MW_lvl1_ypos)
                            End If
                        Else
                            If p11 <> 1 Then 'draw second lvl1 wall left
                                Me.PaintItemL1(mainbmpbig, p11, 1, MW_lvl1_xpos - MW_lvl1_width, MW_lvl1_ypos)
                            End If
                        End If
                        If p01 <> 1 Then
                            If p01 = 0 Then 'wall
                                Me.PaintItemL1(mainbmpbig, p01, 6, SWR_lvl1_xpos, SWR_lvl1_ypos)
                            ElseIf p01 = 9 Or p01 = 7 Or p01 = 6 Or p01 = 5 Then 'door
                                Me.PaintItemL1(mainbmpbig, p01, 6, MW_lvl1_xpos + MW_lvl1_width, MW_lvl1_ypos)
                            End If
                        Else
                            If p13 <> 1 Then 'draw second lvl1 wall left
                                Me.PaintItemL1(mainbmpbig, p13, 3, MW_lvl1_xpos + MW_lvl1_width, MW_lvl1_ypos)
                            End If
                        End If

                    End If
                End If
            End If

            'mainbmp = Me.AddSprite(mainbmp, STAIRS01A, STAIRS_xpos, STAIRS_ypos)
            Me.CopyFromBigGameMap(mainbmp2, mainbmpbig)
            PictureBox1.Image = mainbmp2


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PaintTheWalls(" + px.ToString() + "," + py.ToString + "," + dir + ")")

        End Try
    End Sub


    Public Sub PaintItemL3(ByRef bmp As Bitmap, ByVal item As Integer, ByVal pos As Integer, ByVal px As Integer, ByVal py As Integer)

        Dim x, y As Integer

        Try

            x = px + MainWindow_width
            y = py
            Select Case pos

                Case 0

                    Select Case item
                        Case 0
                            bmp = Me.AddSprite(bmp, WALL_L03, x, y)

                    End Select

                Case 1

                    Select Case item
                        Case 0
                            bmp = Me.AddSprite(bmp, WALL_L03, x, y)
                        Case 5
                            bmp = Me.AddSprite(bmp, STAIRS03A, x, y)
                        Case 6
                            bmp = Me.AddSprite(bmp, STAIRS03B, x, y)
                        Case 7   ', 8
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR03A, x, y)
                        Case 9
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR03B, x, y)

                    End Select
                Case 2
                    Select Case item
                        Case 0
                            bmp = Me.AddSprite(bmp, WALL_L03, x, y)
                        Case 5
                            bmp = Me.AddSprite(bmp, STAIRS03A, x, y)
                        Case 6
                            bmp = Me.AddSprite(bmp, STAIRS03B, x, y)
                        Case 7 ', 8
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR03A, x, y)
                        Case 9
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR03B, x, y)

                    End Select

                Case 3
                    Select Case item
                        Case 0
                            bmp = Me.AddSprite(bmp, WALL_L03, x, y)
                        Case 5
                            bmp = Me.AddSprite(bmp, STAIRS03A, x, y)
                        Case 6
                            bmp = Me.AddSprite(bmp, STAIRS03B, x, y)
                        Case 7 ', 8
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR03A, x, y)
                        Case 9
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR03B, x, y)

                    End Select

                Case 4

                    Select Case item
                        Case 0
                            bmp = Me.AddSprite(bmp, WALL_L03, x, y)
                        Case 5

                    End Select

            End Select

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PaintItemL3(" + px.ToString() + "," + py.ToString + "," + item.ToString + ")")

        End Try


    End Sub

    Public Sub PaintItemL2(ByRef bmp As Bitmap, ByVal item As Integer, ByVal pos As Integer, ByVal px As Integer, ByVal py As Integer)

        Dim x, y As Integer

        Try

            x = px + MainWindow_width
            y = py
            Select Case pos

                Case 0
                    Select Case item
                        Case 0
                            bmp = Me.AddSpriteTransBlack(bmp, SWLEFT_L03, x, y)
                        Case 7, 9, 5, 6
                            bmp = Me.AddSprite(bmp, WALL_L03, x, y)

                    End Select

                Case 1

                    Select Case item
                        Case 0
                            bmp = Me.AddSprite(bmp, WALL_L02, x, y)
                        Case 5
                            bmp = Me.AddSprite(bmp, STAIRS02A, x, y)
                        Case 6
                            bmp = Me.AddSprite(bmp, STAIRS02B, x, y)
                        Case 7   ', 8
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR02A, x, y)
                        Case 9
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR02B, x, y)

                    End Select
                Case 2
                    Select Case item
                        Case 0
                            bmp = Me.AddSprite(bmp, WALL_L02, x, y)
                        Case 5
                            bmp = Me.AddSprite(bmp, STAIRS02A, x, y)
                        Case 6
                            bmp = Me.AddSprite(bmp, STAIRS02B, x, y)

                        Case 7   ', 8
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR02A, x, y)
                        Case 9
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR02B, x, y)


                    End Select

                Case 3
                    Select Case item
                        Case 0
                            bmp = Me.AddSprite(bmp, WALL_L02, x, y)
                        Case 5
                            bmp = Me.AddSprite(bmp, STAIRS02A, x, y)
                        Case 6
                            bmp = Me.AddSprite(bmp, STAIRS02B, x, y)
                        Case 7   ', 8
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR02A, x, y)
                        Case 9
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR02B, x, y)

                    End Select

                Case 4
                    Select Case item
                        Case 0
                            bmp = Me.AddSpriteTransBlack(bmp, SWRIGHT_L03, x, y)
                        Case 7, 9, 5, 6
                            bmp = Me.AddSprite(bmp, WALL_L03, x, y)

                    End Select

            End Select

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PaintItemL2(" + px.ToString() + "," + py.ToString + "," + item.ToString + ")")

        End Try


    End Sub


    Public Sub PaintItemL1(ByRef bmp As Bitmap, ByVal item As Integer, ByVal pos As Integer, ByVal px As Integer, ByVal py As Integer)
        Dim x, y As Integer

        Try

            x = px + MainWindow_width
            y = py
            Select Case pos

                Case 0
                    Select Case item
                        Case 0
                            bmp = Me.AddSpriteTransBlack(bmp, SWLEFT_L02, x, y)
                        Case 7, 9, 5, 6
                            bmp = Me.AddSprite(bmp, WALL_L02, x, y)

                    End Select

                Case 1

                    Select Case item
                        Case 0
                            bmp = Me.AddSprite(bmp, WALL_L01, x, y)
                        Case 5
                            bmp = Me.AddSprite(bmp, STAIRS01A, x, y)
                        Case 6
                            bmp = Me.AddSprite(bmp, STAIRS01B, x, y)
                        Case 7   ', 8
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR01A, x, y)
                        Case 9
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR01B, x, y)

                    End Select
                Case 2
                    Select Case item
                        Case 0
                            bmp = Me.AddSprite(bmp, WALL_L01, x, y)
                        Case 5
                            bmp = Me.AddSprite(bmp, STAIRS01A, x, y)
                        Case 6
                            bmp = Me.AddSprite(bmp, STAIRS01B, x, y)
                        Case 7   ', 8
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR01A, x, y)
                        Case 9
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR01B, x, y)

                        Case 4 'portal
                            'bmp = Me.AddSpriteTransBlack(bmp, TELEPORT, x, y)
                            p_monster.BackgroundImage = PictureBox1.Image
                            p_monster.Visible = True


                    End Select

                Case 3
                    Select Case item
                        Case 0
                            bmp = Me.AddSprite(bmp, WALL_L01, x, y)
                        Case 5
                            bmp = Me.AddSprite(bmp, STAIRS01A, x, y)
                        Case 6
                            bmp = Me.AddSprite(bmp, STAIRS01B, x, y)
                        Case 7   ', 8
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR01A, x, y)
                        Case 9
                            bmp = Me.AddSpriteTransBlack(bmp, DOOR01B, x, y)

                    End Select

                Case 4
                    Select Case item
                        Case 0
                            bmp = Me.AddSpriteTransBlack(bmp, SWRIGHT_L02, x, y)
                        Case 7, 9, 5, 6
                            bmp = Me.AddSprite(bmp, WALL_L02, x, y)

                    End Select

                Case 5 'level 0 sidewall or wall with doors
                    Select Case item
                        Case 0
                            bmp = Me.AddSpriteTransBlack(bmp, SWLEFT_L01, x, y)
                        Case 7, 9, 5, 6
                            bmp = Me.AddSprite(bmp, WALL_L01, x, y)
                            'bmp = Me.AddSprite(bmp, SWLEFT_L01, x, y)

                    End Select

                Case 6 'level 0 sidewall or wall with doors
                    Select Case item
                        Case 0
                            bmp = Me.AddSpriteTransBlack(bmp, SWRIGHT_L01, x, y)
                        Case 7, 9, 5, 6
                            bmp = Me.AddSprite(bmp, WALL_L01, x, y)
                            'bmp = Me.AddSprite(bmp, SWRIGHT_L01, x, y)

                    End Select
            End Select

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PaintItemL1(" + px.ToString() + "," + py.ToString + "," + item.ToString + ")")


        End Try

    End Sub

    Public Function AddSpriteTransWhite(ByRef mypict As Bitmap, ByVal sprite As Bitmap, ByVal xpos As Integer, ByVal ypos As Integer) As Bitmap

        Dim MyColor As Color
        Dim lx, ly, i As Integer
        Dim rect As RectangleF

        Try

            'pixfmt = sprite.GetPixelFormatSize()
            rect = sprite.GetBounds(GraphicsUnit.Pixel)
            lx = rect.Width
            ly = rect.Height
            'sprite.MakeTransparent()
            For x = 0 To lx - 1
                For y = 0 To ly - 1
                    MyColor = sprite.GetPixel(x, y)
                    If MyColor.R = 255 And MyColor.B = 255 And MyColor.G = 255 Then
                        i = 1
                        'nothing
                    Else
                        mypict.SetPixel(xpos + x, ypos + y, MyColor)
                    End If
                Next
            Next

            Return mypict

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "AddSpriteTransWhite(" + xpos.ToString() + "," + ypos.ToString + ")")

        End Try

    End Function


    Public Function AddSpriteTransBlack(ByRef mypict As Bitmap, ByVal sprite As Bitmap, ByVal xpos As Integer, ByVal ypos As Integer) As Bitmap

        Dim MyColor As Color
        Dim lx, ly, i As Integer
        Dim px, py As Integer
        Dim rect1, rect2 As RectangleF

        Try

            rect1 = sprite.GetBounds(GraphicsUnit.Pixel)
            lx = rect1.Width
            ly = rect1.Height
            rect2 = mypict.GetBounds(GraphicsUnit.Pixel)
            px = rect2.Width
            py = rect2.Height
            If lx + xpos > px Then
                MsgBox("Width from picture " + sprite.Tag + " to big")
            End If
            If ly + ypos > py Then
                MsgBox("Height from picture " + sprite.Tag + " to big")
            End If

            For x = 0 To lx - 1
                For y = 0 To ly - 1
                    MyColor = sprite.GetPixel(x, y)
                    If MyColor.R = 0 And MyColor.B = 0 And MyColor.G = 0 Then
                        i = 1
                        'nothing
                    Else
                        mypict.SetPixel(xpos + x, ypos + y, MyColor)
                    End If
                Next
            Next

            Return mypict

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "AddSpriteTransBlack(" + xpos.ToString() + "," + ypos.ToString + ")")

        End Try


    End Function


    Public Function AddSprite(ByRef mypict As Bitmap, ByVal sprite As Bitmap, ByVal xpos As Integer, ByVal ypos As Integer) As Bitmap
        Dim MyColor As Color
        Dim lx, ly As Integer
        Dim px, py As Integer
        Dim rect1, rect2 As RectangleF

        Try
            rect1 = sprite.GetBounds(GraphicsUnit.Pixel)
            lx = rect1.Width
            ly = rect1.Height
            rect2 = mypict.GetBounds(GraphicsUnit.Pixel)
            px = rect2.Width
            py = rect2.Height

            If lx + xpos > px Then
                MsgBox("Width from picture " + sprite.Tag + " to big")
            End If
            If ly + ypos > py Then
                MsgBox("Height from picture " + sprite.Tag + " to big")
            End If

            For x = 0 To lx - 1
                For y = 0 To ly - 1
                    MyColor = sprite.GetPixel(x, y)
                    mypict.SetPixel(xpos + x, ypos + y, MyColor)
                Next
            Next

            Return mypict

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "AddSprite(" + xpos.ToString() + "," + ypos.ToString + ")")

        End Try

    End Function


    Public Sub CopyToBigGameMap(ByRef srcbmp As Bitmap, ByVal trgbmp As Bitmap)
        Dim mycolor As Color
        Dim xpos, ypos As Integer

        xpos = MainWindow_width
        ypos = 0
        Try
            For x = 0 To MainWindow_width - 1
                For y = 0 To MainWindow_height - 1
                    mycolor = srcbmp.GetPixel(x, y)
                    trgbmp.SetPixel(xpos + x, ypos + y, mycolor)
                Next
            Next

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "CopyToBigGameMap")

        End Try

    End Sub

    Public Sub CopyFromBigGameMap(ByRef srcbmp As Bitmap, ByVal trgbmp As Bitmap)
        Dim mycolor As Color
        Dim xpos, ypos As Integer

        xpos = MainWindow_width
        ypos = 0

        Try
            For x = 0 To MainWindow_width - 1
                For y = 0 To MainWindow_height - 1
                    mycolor = trgbmp.GetPixel(x + xpos, y + ypos)
                    srcbmp.SetPixel(x, y, mycolor)
                Next
                'xpos = xpos + 1
            Next

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "CopyFromBigGameMap")

        End Try
    End Sub


    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try

            Select Case CurrentDirection

                Case 0
                    CurrentDirection = 3
                    CurrentView = 3
                    'Me.BackgroundImage = WestBitmap
                    'PBDirection.Image = WestBitmap
                    Me.PaintWalls()

                Case 1
                    CurrentDirection = 0
                    CurrentView = 0
                    'Me.BackgroundImage = NorthBitmap
                    'PBDirection.Image = NorthBitmap
                    Me.PaintWalls()

                Case 2
                    CurrentDirection = 1
                    CurrentView = 1
                    'Me.BackgroundImage = EastBitmap
                    'PBDirection.Image = EastBitmap
                    Me.PaintWalls()

                Case 3
                    CurrentDirection = 2
                    CurrentView = 2
                    'Me.BackgroundImage = SouthBitmap
                    'PBDirection.Image = SouthBitmap
                    Me.PaintWalls()

            End Select

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PictureBox2_Click")

        End Try

    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles T02.Click

        Try

            FightGoingOn = True
            Me.ShowMonster()
            TimeMonster.Enabled = True

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "Button1_Click")

        End Try

    End Sub


    Private Sub FlipControlsNew(ByVal c1 As Integer, ByVal c2 As Integer)

        Dim char1, char2, pbw11, pbw12, pbw21, pbw22 As PictureBox
        Dim lblname1, lblname2 As Label
        Dim h1, h2, pnr, hnr1, hnr2 As Integer
        Dim bmp1, bmp2 As Bitmap
        Dim lbl1, lbl2 As String

        Try

            'Update Controls + Char Classes
            lbl1 = PBNames(c1 - 1).Text
            PBNames(c1 - 1).Text = PBNames(c2 - 1).Text
            GameChars(c1 - 1).Name = PBNames(c2 - 1).Text
            PBNames(c2 - 1).Text = lbl1
            GameChars(c2 - 1).Name = lbl1

            bmp1 = PBPortrait(c1 - 1).Image
            pnr = GameChars(c1 - 1).Picnr
            PBPortrait(c1 - 1).Image = PBPortrait(c2 - 1).Image
            GameChars(c1 - 1).Picnr = GameChars(c2 - 1).Picnr
            PBPortrait(c2 - 1).Image = bmp1
            GameChars(c2 - 1).Picnr = pnr

            bmp1 = HAND1PIC(c1 - 1).Image
            hnr1 = GameChars(c1 - 1).Hand1
            HAND1PIC(c1 - 1).Image = HAND1PIC(c2 - 1).Image
            GameChars(c1 - 1).Hand1 = GameChars(c2 - 1).Hand1
            HAND1PIC(c2 - 1).Image = bmp1
            GameChars(c2 - 1).Hand1 = hnr1

            bmp1 = HAND2PIC(c1 - 1).Image
            hnr2 = GameChars(c1 - 1).Hand2
            HAND2PIC(c1 - 1).Image = HAND2PIC(c2 - 1).Image
            GameChars(c1 - 1).Hand2 = GameChars(c2 - 1).Hand2
            HAND2PIC(c2 - 1).Image = bmp1
            GameChars(c2 - 1).Hand2 = hnr2

            bmp1 = HEALTHPIC(c1 - 1).Image
            h1 = GameChars(c1 - 1).GetHealth()
            HEALTHPIC(c1 - 1).Image = HEALTHPIC(c2 - 1).Image
            GameChars(c1 - 1).Health = GameChars(c2 - 1).Health
            HEALTHPIC(c2 - 1).Image = bmp1
            GameChars(c2 - 1).Health = h1

            'Update Character classes
            GameChars(c1 - 1).ShowHealth()
            GameChars(c2 - 1).ShowHealth()


            'Switch Character Portraits
            'bmp1 = GameChars(c2 - 1).GetPortrait()
            'bmp2 = GameChars(c1 - 1).GetPortrait()
            'GameChars(c1 - 1).UpdatePortrait(bmp1)
            'GameChars(c2 - 1).UpdatePortrait(bmp2)

            'Update Character Numbers in Class
            'GameChars(c1 - 1).UpdateChar(c2)
            'GameChars(c2 - 1).UpdateChar(c1)


            'lblname1.Font = New Font(lblname1.Font, FontStyle.Regular)
            Fliptarget = 0

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "FlipControlsNew(" + c1.ToString() + "," + c2.ToString + ")")


        End Try


    End Sub


    Private Sub FlipControls(ByVal c1 As Integer, ByVal c2 As Integer)

        Dim char1, char2, pbw11, pbw12, pbw21, pbw22, h1, h2 As PictureBox
        Dim lblname1, lblname2 As Label
        Dim x1, y1, x2, y2 As Integer
        Dim bmp1, bmp2 As Bitmap

        Try

            char1 = PBPortrait(c1 - 1)
            lblname1 = PBNames(c1 - 1)
            pbw11 = HAND1PIC(c1 - 1)
            pbw12 = HAND2PIC(c1 - 1)
            h1 = HEALTHPIC(c1 - 1)

            char2 = PBPortrait(c2 - 1)
            lblname2 = PBNames(c2 - 1)
            pbw21 = HAND1PIC(c2 - 1)
            pbw22 = HAND2PIC(c2 - 1)
            h2 = HEALTHPIC(c2 - 1)

            x1 = char1.Location.X
            y1 = char1.Location.Y
            x2 = char2.Location.X
            y2 = char2.Location.Y
            char1.Location = New Point(x2, y2)
            char2.Location = New Point(x1, y1)
            x1 = lblname1.Location.X
            y1 = lblname1.Location.Y
            x2 = lblname2.Location.X
            y2 = lblname2.Location.Y
            lblname1.Location = New Point(x2, y2)
            lblname2.Location = New Point(x1, y1)
            x1 = pbw11.Location.X
            y1 = pbw11.Location.Y
            x2 = pbw21.Location.X
            y2 = pbw21.Location.Y
            pbw11.Location = New Point(x2, y2)
            pbw21.Location = New Point(x1, y1)
            x1 = pbw12.Location.X
            y1 = pbw12.Location.Y
            x2 = pbw22.Location.X
            y2 = pbw22.Location.Y
            pbw12.Location = New Point(x2, y2)
            pbw22.Location = New Point(x1, y1)

            x1 = h1.Location.X
            y1 = h1.Location.Y
            x2 = h2.Location.X
            y2 = h2.Location.Y
            h1.Location = New Point(x2, y2)
            h2.Location = New Point(x1, y1)

            'Switch Character Portraits
            bmp1 = GameChars(c2 - 1).GetPortrait()
            bmp2 = GameChars(c1 - 1).GetPortrait()
            GameChars(c1 - 1).UpdatePortrait(bmp1)
            GameChars(c2 - 1).UpdatePortrait(bmp2)

            'Update Character Numbers in Class
            GameChars(c1 - 1).UpdateChar(c2)
            GameChars(c2 - 1).UpdateChar(c1)


            lblname1.Font = New Font(lblname1.Font, FontStyle.Regular)
            Fliptarget = 0

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "FlipControls(" + c1.ToString() + "," + c2.ToString + ")")

        End Try

    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles T06.Click


        Try
            'PBInventory.Visible = False
            'PBInventory.Location = New Point(Inventory_xpos + Inventory_width, Inventory_ypos)
            Me.RepositionInventory("right")
            'Me.RepositionCharacters("left")


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "Button3_Click")

        End Try

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles T07.Click

        Try

            Me.RepositionInventory("left")
            'Me.RepositionCharacters("right")

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "Button4_Click")

        End Try

    End Sub


    Public Sub RepositionCharacters(ByVal direction As String)

        Try

            If direction = "right" Then
                For i = 0 To 3
                    PBNames(i).Location = New Point(PBNames(i).Location.X + (Inventory_width * 3), PBNames(i).Location.Y)
                    PBPortrait(i).Location = New Point(PBPortrait(i).Location.X + (Inventory_width * 3), PBPortrait(i).Location.Y)
                    HAND1PIC(i).Location = New Point(HAND1PIC(i).Location.X + (Inventory_width * 3), HAND1PIC(i).Location.Y)
                    HAND2PIC(i).Location = New Point(HAND2PIC(i).Location.X + (Inventory_width * 3), HAND2PIC(i).Location.Y)
                    HEALTHPIC(i).Location = New Point(HEALTHPIC(i).Location.X + (Inventory_width * 3), HEALTHPIC(i).Location.Y)
                Next
            ElseIf direction = "left" Then
                For i = 0 To 3
                    PBNames(i).Location = New Point(PBNames(i).Location.X - (Inventory_width * 3), PBNames(i).Location.Y)
                    PBPortrait(i).Location = New Point(PBPortrait(i).Location.X - (Inventory_width * 3), PBPortrait(i).Location.Y)
                    HAND1PIC(i).Location = New Point(HAND1PIC(i).Location.X - (Inventory_width * 3), HAND1PIC(i).Location.Y)
                    HAND2PIC(i).Location = New Point(HAND2PIC(i).Location.X - (Inventory_width * 3), HAND2PIC(i).Location.Y)
                    HEALTHPIC(i).Location = New Point(HEALTHPIC(i).Location.X - (Inventory_width * 3), HEALTHPIC(i).Location.Y)
                Next

            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "RepositionCharacters")


        End Try
    End Sub

    Public Sub RepositionInventory(ByVal direction As String)

        Try

            If direction = "right" Then

                PBInventory.Location = New Point(Inventory_xpos + (Inventory_width * 3), Inventory_ypos)
                For i = 0 To 13
                    PBInv(i).Location = New Point(PBInv(i).Location.X + (Inventory_width * 3), PBInv(i).Location.Y)
                Next
                PBInvUser.Location = New Point(PBInvUser.Location.X + (Inventory_width * 3), PBInvUser.Location.Y)
                'PBInvName.Location = New Point(PBInvName.Location.X + (Inventory_width * 3), PBInvName.Location.Y)
            ElseIf direction = "left" Then
                PBInventory.Location = New Point(Inventory_xpos, Inventory_ypos)
                For i = 0 To 13
                    PBInv(i).Location = New Point(PBInv(i).Location.X - (Inventory_width * 3), PBInv(i).Location.Y)
                Next
                PBInvUser.Location = New Point(PBInvUser.Location.X - (Inventory_width * 3), PBInvUser.Location.Y)
                'PBInvName.Location = New Point(PBInvName.Location.X - (Inventory_width * 3), PBInvName.Location.Y)

            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "RepositionInventory")

        End Try

    End Sub

    Public Function Getitem(ByVal itemnr As Integer) As Bitmap
        Dim bmp As Bitmap
        Dim pictname, filename As String

        Try

            pictname = Me.GetItemPict(itemnr)
            filename = GameFile + "\items\" + pictname
            bmp = Bitmap.FromFile(filename)
            Return bmp

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "Getitem(" + itemnr.ToString + ")")

        End Try

    End Function


    Private Sub Mainwindow2_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        Dim px, py, MyControl As Integer

        Try

            px = e.X
            py = e.Y
            MyControl = Me.CheckClickedPosition(px, py)

            Select Case MyControl
                Case 0
                    Return
                Case 1
                    Me.ClickLeftturn()
                Case 2
                    Me.ClickLeft()
                Case 3
                    Me.ClickUp()
                Case 4
                    Me.ClickDown()
                Case 5
                    Me.ClickRightturn()
                Case 6
                    Me.ClickRight()


            End Select

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "Mainwindow2_MouseClick(" + px.ToString + "," + py.ToString + ")")


        End Try

    End Sub


    Public Function CheckClickedPosition(ByVal x As Integer, ByVal y As Integer) As Integer
        Dim cntrl = 0

        Try

            'Check Control LeftTurn
            If x > leftturn_xpos And x < leftturn_xpos + leftturn_width And y > leftturn_ypos And y < leftturn_ypos + leftturn_height Then
                cntrl = 1
                Return cntrl
            End If
            'Check Control Left (west)
            If x > left_xpos And x < left_xpos + leftturn_width And y > left_ypos And y < left_ypos + leftturn_height Then
                cntrl = 2
                Return cntrl
            End If
            'Check Control Up (north)
            If x > up_xpos And x < up_xpos + leftturn_width And y > up_ypos And y < up_ypos + leftturn_height Then
                cntrl = 3
                Return cntrl
            End If
            'Check Control Down (south)
            If x > down_xpos And x < down_xpos + leftturn_width And y > down_ypos And y < down_ypos + leftturn_height Then
                cntrl = 4
                Return cntrl
            End If
            'Check Control RightTurn
            If x > rightturn_xpos And x < rightturn_xpos + leftturn_width And y > rightturn_ypos And y < rightturn_ypos + leftturn_height Then
                cntrl = 5
                Return cntrl
            End If
            'Check Control Right (east)
            If x > right_xpos And x < right_xpos + leftturn_width And y > right_ypos And y < right_ypos + leftturn_height Then
                cntrl = 6
                Return cntrl
            End If

            Return cntrl

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "CheckClickedPosition(" + x.ToString + "," + y.ToString + ")")

            cntrl = 3
            Return cntrl

        End Try


    End Function


    Private Sub PictureBox1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseClick
        Dim x, y, curitem As Integer
        Dim posx, posy, doorx, doory As Integer
        Dim bmp As Bitmap

        Try

            x = e.X
            y = e.Y
            Select Case CurrentView
                Case 0 'North
                    posx = CurrentPosX
                    posy = CurrentPosY - 1
                Case 1 'East
                    posx = CurrentPosX + 1
                    posy = CurrentPosY
                Case 2 'South
                    posx = CurrentPosX
                    posy = CurrentPosY + 1
                Case 3 'West
                    posx = CurrentPosX - 1
                    posy = CurrentPosY
            End Select

            'check the clicked area
            'check the area on wall for key operation
            'MsgBox("L" + CurrentLevel.ToString() + "'" + posx.ToString() + "'" + posy.ToString())

            If x > ItemWall_xpos And x < ItemWall_xpos + ItemWall_width And y > ItemWall_ypos And y < ItemWall_ypos + ItemWall_height Then
                'Is the current dragged item a key ?
                If DragDropStarted = True And DraggedItem < 50 Then
                    'is there a keyhole with the same code as the dragged key ? (keyholecode - 50 = keycode)
                    curitem = MyItemsWall(posx, posy)
                    If curitem - 50 = DraggedItem Then 'this is correct key
                        'MsgBox("You clicked the keyhole area and this is the correct key !")
                        Me.GetDoorPosition(doorx, doory, CurrentLevel, posx, posy)
                        MyFields(doorx, doory) = 9
                        MyFieldsB(doorx + 3, doory + 3) = 9

                        Me.Cursor = Windows.Forms.Cursors.Arrow
                        DragDropStarted = False
                        Me.Refresh()
                    End If
                End If
            End If

            'check the area on floor for drag operation
            If x > ItemFloor_xpos And x < ItemFloor_xpos + ItemFloor_width And y > ItemFloor_ypos And y < ItemFloor_ypos + ItemFloor_height Then
                'check if there is an item lying there
                curitem = MyItemsFloor(posx, posy)

                'check later if the item is draggable ! Skip for now !
                If DragDropStarted = False Then
                    If curitem > 0 Then 'item on ground start drag
                        SourceDragControl = PictureBox1
                        DraggedItem = curitem
                        bmp = Me.Getitem(DraggedItem)
                        Me.NewCursor(bmp, 30, 30)
                        'Update array Myitems and bitmap Picturebox1
                        MyItemsFloor(posx, posy) = 0
                        Me.PaintWalls()
                        Me.PaintItems()
                        DragDropStarted = True
                    Else
                        Return
                    End If

                Else 'Dragdropactive item is dropped here
                    'Update array Myitems and bitmap Picturebox1
                    MyItemsFloor(posx, posy) = DraggedItem
                    Me.PaintItems()
                    Me.Cursor = Windows.Forms.Cursors.Arrow
                    DragDropStarted = False
                    Me.Refresh()

                End If

            Else
                Return
            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PictureBox1_MouseClick(" + x.ToString + "," + y.ToString + ")")

        End Try


    End Sub

    Public Sub GetDoorPosition(ByRef doorx As Integer, ByRef doory As Integer, ByVal mylvl As Integer, ByVal posx As Integer, ByVal posy As Integer)

        Dim mysql As String
        Dim mystring As String
        Dim myconnection1 As DbConnection
        Dim mycommand As DbCommand

        Try

            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            mysql = "select posx from Doors where gameid = 1 and levelid = " + mylvl.ToString() + " and lockposx = " + posx.ToString() + " and lockposy = " + posy.ToString()

            mycommand = New OleDbCommand
            mycommand.CommandText = mysql
            mycommand.Connection = myconnection1
            myconnection1.Open()
            doorx = mycommand.ExecuteScalar()

            mysql = "select posy from Doors where gameid = 1 and levelid = " + mylvl.ToString() + " and lockposx = " + posx.ToString() + " and lockposy = " + posy.ToString()
            mycommand.CommandText = mysql
            doory = mycommand.ExecuteScalar()

            myconnection1.Close()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "GetDoorPosition(" + mylvl.ToString + "," + posx.ToString + "," + posy.ToString + ")")


        End Try

    End Sub


    Private Sub bAbort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bAbort.Click

        Try
            PictureBox2.Visible = True
            SpellsVisible = False

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "bAbort_Click")

        End Try

    End Sub

    Private Function CropBitmap(ByRef bmp As Bitmap, ByVal cropX As Integer, ByVal cropY As Integer, ByVal cropWidth As Integer, ByVal cropHeight As Integer) As Bitmap

        Try

            Dim rect As New Rectangle(cropX, cropY, cropWidth, cropHeight)
            Dim cropped As Bitmap = bmp.Clone(rect, bmp.PixelFormat)
            Return cropped

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "CropBitmap")

        End Try


    End Function



    Public Sub FillItemsWallDB(ByVal mapnr As Integer)
        Dim itemstr, curitem As String
        Dim ind As Integer

        Try

            itemstr = Me.GetWallItemstring(mapnr)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyItemsWall(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "FillItemsWallDB(" + mapnr.ToString + ")")

        End Try
    End Sub

    Public Function GetWallItemstring(ByVal mapnr As Integer) As String

        Dim mysql As String
        Dim mystring, theitem As String
        Dim myconnection1 As DbConnection
        Dim mycommand As DbCommand


        Try

            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            mysql = "select wallitems from Wallitems where mapnr = " + mapnr.ToString()

            mycommand = New OleDbCommand
            mycommand.CommandText = mysql
            mycommand.Connection = myconnection1
            myconnection1.Open()
            theitem = mycommand.ExecuteScalar()
            myconnection1.Close()

            Return theitem

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "GetWallItemstring(" + mapnr.ToString + ")")

            Return ""

        End Try


    End Function

    Public Sub FillMonstersDB()

        Dim itemstr, curitem As String
        Dim ind As Integer

        Try

            'get monsters level 1
            itemstr = Me.GetMonsterstring(1)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters1(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 2
            itemstr = Me.GetMonsterstring(2)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters2(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 3
            itemstr = Me.GetMonsterstring(3)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters3(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 4
            itemstr = Me.GetMonsterstring(4)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters4(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 5
            itemstr = Me.GetMonsterstring(5)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters5(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 6
            itemstr = Me.GetMonsterstring(6)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters6(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 7
            itemstr = Me.GetMonsterstring(7)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters7(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 8
            itemstr = Me.GetMonsterstring(8)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters8(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 9
            itemstr = Me.GetMonsterstring(9)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters9(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 10
            itemstr = Me.GetMonsterstring(10)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters10(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 11
            itemstr = Me.GetMonsterstring(11)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters11(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 12
            itemstr = Me.GetMonsterstring(12)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters12(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 13
            itemstr = Me.GetMonsterstring(13)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters13(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 14
            itemstr = Me.GetMonsterstring(14)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters14(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 15
            itemstr = Me.GetMonsterstring(15)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters15(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            'get monsters level 16
            itemstr = Me.GetMonsterstring(16)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyMonsters16(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "FillMonstersDB")

        End Try



    End Sub

    Public Sub FillMonstersDB(ByVal mapnr As Integer)

        Dim itemstr, curitem As String
        Dim ind As Integer

        Try

            Select Case mapnr

                Case 1
                    MyMonsters = MyMonsters1

                Case 2
                    MyMonsters = MyMonsters2

                Case 3
                    MyMonsters = MyMonsters3

                Case 4
                    MyMonsters = MyMonsters4

                Case 5
                    MyMonsters = MyMonsters5

                Case 6
                    MyMonsters = MyMonsters6

                Case 7
                    MyMonsters = MyMonsters7

                Case 8
                    MyMonsters = MyMonsters8

                Case 9
                    MyMonsters = MyMonsters9

                Case 10
                    MyMonsters = MyMonsters10

                Case 11
                    MyMonsters = MyMonsters11

                Case 12
                    MyMonsters = MyMonsters12

                Case 13
                    MyMonsters = MyMonsters13

                Case 14
                    MyMonsters = MyMonsters14

                Case 15
                    MyMonsters = MyMonsters15

                Case 16
                    MyMonsters = MyMonsters16

            End Select


            ''show the possible items on map
            'itemstr = Me.GetMonsterstring(mapnr)
            'ind = 0
            'For y = 1 To 32
            '    For x = 1 To 32
            '        curitem = itemstr.Substring(ind, 3)
            '        MyMonsters(x, y) = CInt(curitem)
            '        ind = ind + 4 'skip the , character
            '    Next
            '    'ind = ind + 2 'skip the vbcrlf characters
            'Next

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "FillMonstersDB(" + mapnr.ToString() + ")")

        End Try

    End Sub

    Public Sub FillItemsDB(ByVal mapnr As Integer)
        Dim itemstr, curitem As String
        Dim ind As Integer

        Try

            'show the possible items on map
            itemstr = Me.GetItemstring(mapnr)
            ind = 0
            For y = 1 To 32
                For x = 1 To 32
                    curitem = itemstr.Substring(ind, 3)
                    MyItemsFloor(x, y) = CInt(curitem)
                    ind = ind + 4 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "FillItemsDB(" + mapnr.ToString + ")")

        End Try

    End Sub

    Public Function GetMonsterstring(ByVal mapnr As Integer) As String

        Dim mysql As String
        Dim mystring, theitem As String
        Dim myconnection1 As DbConnection
        Dim mycommand As DbCommand

        Try

            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            mysql = "select items from Mapmonsters where mapnr = " + mapnr.ToString()


            mycommand = New OleDbCommand
            mycommand.CommandText = mysql
            mycommand.Connection = myconnection1
            myconnection1.Open()
            theitem = mycommand.ExecuteScalar()
            myconnection1.Close()

            Return theitem

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "GetMonsterstring(" + mapnr.ToString + ")")

            Return ""

        End Try


    End Function

    Public Function GetItemstring(ByVal mapnr As Integer) As String

        Dim mysql As String
        Dim mystring, theitem As String
        Dim myconnection1 As DbConnection
        Dim mycommand As DbCommand

        Try

            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            mysql = "select items from Mapitems where mapnr = " + mapnr.ToString()


            mycommand = New OleDbCommand
            mycommand.CommandText = mysql
            mycommand.Connection = myconnection1
            myconnection1.Open()
            theitem = mycommand.ExecuteScalar()
            myconnection1.Close()

            Return theitem

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "GetItemstring(" + mapnr.ToString + ")")

            Return ""

        End Try

    End Function


    Public Sub FillMapsDB(ByVal mapnr As Integer)
        Dim dsmaps As New DataSet
        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection
        Dim mapstr, curitem As String
        Dim ind As Integer

        Try

            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            da = New OleDbDataAdapter
            mysql = "select * from Maps"
            da.SelectCommand = New OleDbCommand(mysql, myconnection1)

            myconnection1.Open()

            da.Fill(dsmaps, "Maps")

            For x = 1 To 38
                For y = 1 To 38
                    MyFieldsB(x, y) = 0
                Next
            Next
            ind = 0
            mapstr = dsmaps.Tables(0).Rows(mapnr).Item(5)
            For y = 1 To 32
                For x = 1 To 32
                    curitem = mapstr.Substring(ind, 1)
                    MyFields(x, y) = CInt(curitem)
                    ind = ind + 2 'skip the , character
                Next
                'ind = ind + 2 'skip the vbcrlf characters
            Next

            For x = 1 To 32
                For y = 1 To 32
                    MyFieldsB(x + 3, y + 3) = MyFields(x, y)
                Next
            Next
            myconnection1.Close()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "FillMapsDB(" + mapnr.ToString() + ")")

        End Try


    End Sub

    Public Sub PaintTheItemWall(ByVal theitem As Integer, ByVal Lvl As Integer)

        Dim bmp, bmpField As Bitmap
        Dim pictname, filename As String

        Try

            'select pictname from table Itemtypes
            pictname = Me.GetItemPict(theitem)
            filename = GameFile + "\items\" + pictname
            bmp = Bitmap.FromFile(filename)
            bmpField = PictureBox1.Image

            If Lvl = 3 Then
                Dim bm_dest As New Bitmap(CInt(bmp.Width * 0.5), CInt(bmp.Height * 0.5))
                Dim gr_dest As Graphics = Graphics.FromImage(bm_dest)
                gr_dest.DrawImage(bmp, 0, 0, bm_dest.Width + 1, bm_dest.Height + 1)
                'bm_dest = Me.BlurPict(0.3, bm_dest)

                bmpField = Me.AddSpriteTransBlack(bmpField, bm_dest, ItemWall_xpos3, ItemWall_ypos3)
            ElseIf Lvl = 2 Then
                Dim bm_dest As New Bitmap(CInt(bmp.Width * 0.7), CInt(bmp.Height * 0.7))
                Dim gr_dest As Graphics = Graphics.FromImage(bm_dest)
                gr_dest.DrawImage(bmp, 0, 0, bm_dest.Width + 1, bm_dest.Height + 1)
                bmpField = Me.AddSpriteTransBlack(bmpField, bm_dest, ItemWall_xpos2, ItemWall_ypos2)
            Else
                bmpField = Me.AddSpriteTransBlack(bmpField, bmp, ItemWall_xpos, ItemWall_ypos)
            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PaintTheItemWall(" + theitem.ToString + "," + Lvl.ToString + ")")

        End Try

        'bmp.MakeTransparent(Color.Black)

    End Sub

    Public Sub PaintTheItem(ByVal theitem As Integer)

        Dim bmp, bmpField As Bitmap
        Dim pictname, filename As String

        Try

            'Try the special bitmap BITMAP2.BMP first, if not use BITMAP.BMP
            'select pictname from table Itemtypes
            pictname = Me.GetItemPict(theitem)
            filename = GameFile + "\items\" + pictname
            bmp = Bitmap.FromFile(filename)
            'bmp.MakeTransparent(Color.Black)

            bmpField = PictureBox1.Image
            bmpField = Me.AddSpriteTransBlack(bmpField, bmp, ItemFloor_xpos, ItemFloor_ypos)

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PaintTheItemWall(" + theitem.ToString + ")")

        End Try


    End Sub

    Public Function GetItemPict(ByVal itemnr As Integer) As String

        Dim mysql As String
        Dim mystring, itemname As String
        Dim myconnection1 As DbConnection
        Dim mycommand As DbCommand

        Try

            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            mysql = "select pictname from Itemtypes where itemnr = " + itemnr.ToString()

            mycommand = New OleDbCommand
            mycommand.CommandText = mysql
            mycommand.Connection = myconnection1
            myconnection1.Open()
            itemname = mycommand.ExecuteScalar()

            myconnection1.Close()

            Return itemname

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "GetItemPict")
            LogMessage(ex.Message, "GetItemPict(" + itemnr.ToString + ")")

            Return ""

        End Try

    End Function


    Public Sub PaintItems()
        Dim posx, posx2, posx3, posy, posy2, posy3 As Integer
        Dim curitem, curitemwall, curmonster As Integer

        Try

            Select Case CurrentView

                Case 0 'North
                    If CurrentPosY > 0 Then
                        posx = CurrentPosX
                        posy = CurrentPosY - 1
                        If CurrentPosY > 1 Then
                            posy2 = CurrentPosY - 2
                            If CurrentPosY > 2 Then
                                posy3 = CurrentPosY - 3
                            End If
                        End If
                    Else
                        Return
                    End If

                Case 1 'East
                    If CurrentPosX < 31 Then
                        posx = CurrentPosX + 1
                        posy = CurrentPosY
                        If CurrentPosX < 30 Then
                            posx2 = CurrentPosX + 2
                            If CurrentPosX < 29 Then
                                posx3 = CurrentPosX + 3
                            End If
                        End If
                    Else
                        Return
                    End If

                Case 2 'South
                    If CurrentPosY < 31 Then
                        posx = CurrentPosX
                        posy = CurrentPosY + 1
                        If CurrentPosY < 30 Then
                            posy2 = CurrentPosY + 2
                            If CurrentPosY < 29 Then
                                posy3 = CurrentPosY + 3
                            End If
                        End If
                    Else
                        Return
                    End If

                Case 3 'West
                    If CurrentPosX > 0 Then
                        posx = CurrentPosX - 1
                        posy = CurrentPosY
                        If CurrentPosX > 1 Then
                            posx2 = CurrentPosX - 2
                            If CurrentPosX > 2 Then
                                posx3 = CurrentPosX - 3
                            End If
                        End If
                    Else
                        Return
                    End If

            End Select
            curitem = MyItemsFloor(posx, posy)
            If curitem > 0 Then
                Me.PaintTheItem(curitem)
            End If

            curitemwall = MyItemsWall(posx, posy)
            If curitemwall > 0 Then
                Me.PaintTheItemWall(curitemwall, 1)
            Else 'check level 2
                If posy2 > 0 And MyFields(posx, posy) = 1 And MyItemsWall(posx, posy2) > 0 Then
                    curitemwall = MyItemsWall(posx, posy2)
                    Me.PaintTheItemWall(curitemwall, 2)
                ElseIf posx2 > 0 And MyFields(posx, posy) = 1 And MyItemsWall(posx2, posy) > 0 Then
                    curitemwall = MyItemsWall(posx2, posy)
                    Me.PaintTheItemWall(curitemwall, 2)
                ElseIf posy3 > 0 And MyFields(posx, posy) = 1 And MyFields(posx, posy2) = 1 And MyItemsWall(posx, posy3) > 0 Then
                    curitemwall = MyItemsWall(posx, posy3)
                    Me.PaintTheItemWall(curitemwall, 3)
                ElseIf posx3 > 0 And MyFields(posx, posy) = 1 And MyFields(posx2, posy) = 1 And MyItemsWall(posx3, posy) > 0 Then
                    curitemwall = MyItemsWall(posx3, posy)
                    Me.PaintTheItemWall(curitemwall, 3)
                End If

                'If posy3 > 0 Or posx3 > 0 Then
                '    If posy3 > 0 And MyFields(posx, posy) = 1 And MyFields(posx, posy2) = 1 Then
                '        curitemwall = MyItemsWall(posx, posy3)
                '        If curitemwall > 0 Then
                '            Me.PaintTheItemWall(curitemwall, 3)
                '        End If

                '    End If
                '    If posx3 > 0 And MyFields(posx, posy) = 1 And MyFields(posx2, posy) = 1 Then
                '        curitemwall = MyItemsWall(posx3, posy)
                '        If curitemwall > 0 Then
                '            Me.PaintTheItemWall(curitemwall, 3)
                '        End If
                '    End If
                'End If

            End If

            'If posy2 > 0 Or posx2 > 0 Then
            '    If posy2 > 0 And MyFields(posx, posy) = 1 Then
            '        curitemwall = MyItemsWall(posx, posy2)
            '        If curitemwall > 0 Then
            '            Me.PaintTheItemWall(curitemwall, 2)
            '        End If
            '    End If
            '    If posx2 > 0 And MyFields(posx, posy) = 1 Then
            '        curitemwall = MyItemsWall(posx2, posy)
            '        If curitemwall > 0 Then
            '            Me.PaintTheItemWall(curitemwall, 2)
            '        End If
            '    End If
            'End If

            curmonster = MyMonsters(posx, posy)
            If curmonster > 0 Then
                PosMonsterX = posx
                PosMonsterY = posy
                FightGoingOn = True
                Me.ShowMonster(curmonster)
                TimeMonster.Enabled = True
            Else
                If posy2 > 0 Or posx2 > 0 Then
                    If posy2 > 0 And MyFields(posx, posy) = 1 Then
                        curmonster = MyMonsters(posx, posy2)
                        If curmonster > 0 Then
                            Me.ShowMonsterL2(curmonster)
                        End If
                    End If

                    If posx2 > 0 And MyFields(posx, posy) = 1 Then
                        curmonster = MyMonsters(posx2, posy)
                        If curmonster > 0 Then
                            Me.ShowMonsterL2(curmonster)
                        End If
                    End If

                End If

            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PaintItems")

        End Try

    End Sub

    Public Sub PaintItemsOld()
        Dim posx, posy As Integer
        Dim curitem, curitemwall, curmonster As Integer

        Try

            Select Case CurrentView

                Case 0 'North
                    If CurrentPosY > 0 Then
                        posx = CurrentPosX
                        posy = CurrentPosY - 1
                    Else
                        Return
                    End If

                Case 1 'East
                    If CurrentPosX < 31 Then
                        posx = CurrentPosX + 1
                        posy = CurrentPosY
                    Else
                        Return
                    End If

                Case 2 'South
                    If CurrentPosY < 31 Then
                        posx = CurrentPosX
                        posy = CurrentPosY + 1
                    Else
                        Return
                    End If

                Case 3 'West
                    If CurrentPosX > 0 Then
                        posx = CurrentPosX - 1
                        posy = CurrentPosY
                    Else
                        Return
                    End If

            End Select
            curitem = MyItemsFloor(posx, posy)
            If curitem > 0 Then
                Me.PaintTheItem(curitem)
            End If
            curitemwall = MyItemsWall(posx, posy)
            If curitemwall > 0 Then
                Me.PaintTheItemWall(curitemwall, 1)
            End If
            curmonster = MyMonsters(posx, posy)
            If curmonster > 0 Then
                PosMonsterX = posx
                PosMonsterY = posy
                FightGoingOn = True
                Me.ShowMonster(curmonster)
                TimeMonster.Enabled = True

            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PaintItemsOld")

        End Try


    End Sub

    Public Sub LoadMonster2()

        Try

            MONSTERPIC1 = Bitmap.FromFile(CurDir() + "\monsters\" + MONSTERPICNAME2)

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadMonster2")

        End Try

    End Sub

    Public Sub LoadMonster()

        Try

            MONSTERPIC1 = Bitmap.FromFile(CurDir() + "\monsters\" + MONSTERPICNAME1)

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "LoadMonster")

        End Try

    End Sub

    Public Sub ShowMonster3()

        Try

            HEALTHMONSTER.Width = (CurrentHealth / 100) * healthmonster_width
            HEALTHMONSTER.Visible = True
            p_health2.Visible = True

            'MONSTERPICNAME1 = "wolf3.bmp"
            'MONSTERPICNAME1 = T03.Text + "1.bmp"
            MONSTERPICNAME1 = T03.Text + "anim.gif"

            Me.LoadMonster()
            Dim bmp1 As Bitmap = MONSTERPIC1
            'bmp1.MakeTransparent(Color.Black)
            p_monster.Image = bmp1

            p_monster.BackgroundImage = PictureBox1.Image
            p_monster.Visible = True

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ShowMonster3")

        End Try

    End Sub

    Public Sub ShowMonster2()


        Try

            HEALTHMONSTER.Width = (CurrentHealth / 100) * healthmonster_width
            HEALTHMONSTER.Visible = True
            p_health2.Visible = True

            'MONSTERPICNAME1 = "wolf2.bmp"
            'MONSTERPICNAME1 = T03.Text + "2.bmp"
            MONSTERPICNAME1 = T03.Text + "anim.gif"

            Me.LoadMonster()
            Dim bmp1 As Bitmap = MONSTERPIC1
            'bmp1.MakeTransparent(Color.Black)
            p_monster.Image = bmp1

            p_monster.BackgroundImage = PictureBox1.Image
            p_monster.Visible = True

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ShowMonster2")

        End Try

    End Sub

    Public Sub ShowMonsterL2(ByVal monsternr As Integer)

        Try

            Select Case monsternr
                Case 1
                    MONSTERPICNAME1 = "gargoyleanim.gif"
                    MONSTERPICNAME2 = "gargoyle1.bmp"

                Case 2
                    MONSTERPICNAME1 = "antanim.gif"
                    MONSTERPICNAME2 = "ant1.bmp"
                Case 3
                    MONSTERPICNAME1 = "aservantanim.gif"
                    MONSTERPICNAME2 = "aservant1.bmp"
                Case 4
                    MONSTERPICNAME1 = "basiliskanim.gif"
                    MONSTERPICNAME2 = "basilisk1.bmp"
                Case 5
                    MONSTERPICNAME1 = "beholderanim.gif"
                    MONSTERPICNAME2 = "beholder1.bmp"
                Case 6
                    MONSTERPICNAME1 = "buletteanim.gif"
                    MONSTERPICNAME2 = "bullette1.bmp"
                Case 7
                    MONSTERPICNAME1 = "clericanim.gif"
                    MONSTERPICNAME2 = "cleric1.bmp"
                Case 8
                    MONSTERPICNAME1 = "cubeanim.gif"
                    MONSTERPICNAME2 = "cube1.bmp"
                Case 9
                    MONSTERPICNAME1 = "dragonanim.gif"
                    MONSTERPICNAME2 = "dragon1.bmp"
                Case 10
                    MONSTERPICNAME1 = "giantanim.gif"
                    MONSTERPICNAME2 = "giant1.bmp"
                Case 11
                    MONSTERPICNAME1 = "guardanim.gif"
                    MONSTERPICNAME2 = "guard1.bmp"
                Case 12
                    MONSTERPICNAME1 = "guardiananim.gif"
                    MONSTERPICNAME2 = "guardian1.bmp"
                Case 13
                    MONSTERPICNAME1 = "hellhoundanim.gif"
                    MONSTERPICNAME2 = "hellhound1"
                Case 14
                    MONSTERPICNAME1 = "mageanim.gif"
                    MONSTERPICNAME2 = "mage1"
                Case 15
                    MONSTERPICNAME1 = "mantisanim.gif"
                    MONSTERPICNAME2 = "mantis1.bmp"
                Case 16
                    MONSTERPICNAME1 = "medusaanim.gif"
                    MONSTERPICNAME2 = "medusa1.bmp"
                Case 17
                    MONSTERPICNAME1 = "mindflayeranim.gif"
                    MONSTERPICNAME2 = "mindflayer1.bmp"
                Case 18
                    MONSTERPICNAME1 = "salamanderanim.gif"
                    MONSTERPICNAME2 = "salamander1.bmp"
                Case 19
                    MONSTERPICNAME1 = "skeletwarrioranim.gif"
                    MONSTERPICNAME2 = "skeletwarrior1.bmp"
                Case 20
                    MONSTERPICNAME1 = "snakeanim.gif"
                    MONSTERPICNAME2 = "snake1.bmp"
                Case 21
                    MONSTERPICNAME1 = "spideranim.gif"
                    MONSTERPICNAME2 = "spider1.bmp"
                Case 22
                    MONSTERPICNAME1 = "waspanim.gif"
                    MONSTERPICNAME2 = "wasp1.bmp"
                Case 23
                    MONSTERPICNAME1 = "willowwispanim.gif"
                    MONSTERPICNAME2 = "willowwisp1.bmp"
                Case 24
                    MONSTERPICNAME1 = "wolfanim.gif"
                    MONSTERPICNAME2 = "wolf1.bmp"

            End Select

            'MONSTERPICNAME1 = T03.Text + "anim.gif"
            Me.LoadMonster2()
            Dim bmp1 As Bitmap = MONSTERPIC1
            bmp1.MakeTransparent()

            p_monster.Image = bmp1
            p_monster.BackgroundImage = PictureBox1.Image
            p_monster.Visible = True

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ShowMonster2")
            LogMessage(ex.Message, "ShowMonster2(" + monsternr.ToString + ")")

        End Try

    End Sub

    Public Sub ShowMonster(ByVal monsternr As Integer)

        Try

            HEALTHMONSTER.Width = (CurrentHealth / 100) * healthmonster_width
            HEALTHMONSTER.Visible = True
            p_health2.Visible = True

            Select Case monsternr
                Case 1
                    MONSTERPICNAME1 = "gargoyleanim.gif"
                Case 2
                    MONSTERPICNAME1 = "antanim.gif"
                Case 3
                    MONSTERPICNAME1 = "aservantanim.gif"
                Case 4
                    MONSTERPICNAME1 = "basiliskanim.gif"
                Case 5
                    MONSTERPICNAME1 = "beholderanim.gif"
                Case 6
                    MONSTERPICNAME1 = "buletteanim.gif"
                Case 7
                    MONSTERPICNAME1 = "clericanim.gif"
                Case 8
                    MONSTERPICNAME1 = "cubeanim.gif"
                Case 9
                    MONSTERPICNAME1 = "dragonanim.gif"
                Case 10
                    MONSTERPICNAME1 = "giantanim.gif"
                Case 11
                    MONSTERPICNAME1 = "guardanim.gif"
                Case 12
                    MONSTERPICNAME1 = "guardiananim.gif"
                Case 13
                    MONSTERPICNAME1 = "hellhoundanim.gif"
                Case 14
                    MONSTERPICNAME1 = "mageanim.gif"
                Case 15
                    MONSTERPICNAME1 = "mantisanim.gif"
                Case 16
                    MONSTERPICNAME1 = "medusaanim.gif"
                Case 17
                    MONSTERPICNAME1 = "mindflayeranim.gif"
                Case 18
                    MONSTERPICNAME1 = "salamanderanim.gif"
                Case 19
                    MONSTERPICNAME1 = "skeletwarrioranim.gif"
                Case 20
                    MONSTERPICNAME1 = "snakeanim.gif"
                Case 21
                    MONSTERPICNAME1 = "spideranim.gif"
                Case 22
                    MONSTERPICNAME1 = "waspanim.gif"
                Case 23
                    MONSTERPICNAME1 = "willowwispanim.gif"
                Case 24
                    MONSTERPICNAME1 = "wolfanim.gif"

            End Select
            'MONSTERPICNAME1 = T03.Text + "anim.gif"
            Me.LoadMonster()

            Dim bmp1 As Bitmap = MONSTERPIC1
            p_monster.Image = bmp1

            p_monster.BackgroundImage = PictureBox1.Image
            p_monster.Visible = True

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ShowMonster2(" + monsternr.ToString + ")")

        End Try

    End Sub

    Public Sub ShowMonster()

        Try

            HEALTHMONSTER.Width = (CurrentHealth / 100) * healthmonster_width
            HEALTHMONSTER.Visible = True
            p_health2.Visible = True

            'MONSTERPICNAME1 = "wolf1.bmp"
            MONSTERPICNAME1 = T03.Text + "anim.gif"
            'MONSTERPICNAME1 = "gargoyleanim.gif"

            Me.LoadMonster()

            Dim bmp1 As Bitmap = MONSTERPIC1
            'bmp1.MakeTransparent(Color.Black)
            p_monster.Image = bmp1
            'p_monster.BackColor = Color.Transparent

            p_monster.BackgroundImage = PictureBox1.Image
            p_monster.Visible = True

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ShowMonster")

        End Try

    End Sub


    Public Sub GetNewPositions(ByVal mylvl As Integer, ByVal posx As Integer, ByVal posy As Integer, ByRef newlvl As Integer, ByRef newx As Integer, ByRef newy As Integer)
        Dim mysql As String
        Dim mystring As String
        Dim myconnection1 As DbConnection
        Dim mycommand As DbCommand

        Try

            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)

            mysql = "select target from Stairs where gameid = 1 and levelid = " + mylvl.ToString() + " and posx = " + posx.ToString() + " and posy = " + posy.ToString()

            mycommand = New OleDbCommand
            mycommand.CommandText = mysql
            mycommand.Connection = myconnection1
            myconnection1.Open()
            newlvl = mycommand.ExecuteScalar()

            mysql = "select tposx from Stairs where gameid = 1 and levelid = " + mylvl.ToString() + " and posx = " + posx.ToString() + " and posy = " + posy.ToString()
            mycommand.CommandText = mysql
            newx = mycommand.ExecuteScalar()

            mysql = "select tposy from Stairs where gameid = 1 and levelid = " + mylvl.ToString() + " and posx = " + posx.ToString() + " and posy = " + posy.ToString()
            mycommand.CommandText = mysql
            newy = mycommand.ExecuteScalar()

            myconnection1.Close()

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "GetNewPositions(" + mylvl.ToString + "," + posx.ToString + "," + posy.ToString + ")")

        End Try

    End Sub


    Private Sub PictureBox3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox3.Click

        Try

            If MonsterDebugging = False Then
                T01.Visible = True
                T02.Visible = True
                T03.Visible = True
                T04.Visible = True
                T05.Visible = True
                MonsterDebugging = True
            Else
                T01.Visible = False
                T02.Visible = False
                T03.Visible = False
                T04.Visible = False
                T05.Visible = False
                MonsterDebugging = False
            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "PictureBox3_Click")

        End Try


    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeMonster.Tick
        Dim hits, thechar As Integer

        Try

            'Me.ShowMonster2()
            TimeBitmap.Enabled = True

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "Timer1_Tick")

        End Try


    End Sub


    Private Sub ClickHand2(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim str As String
        Dim ind, ind1, ind2, Hits, health As Integer
        Dim btn As PictureBox
        Dim bmp As Bitmap
        Dim MyHitsNew As Date
        Dim weapon As Integer

        Try

            btn = CType(sender, PictureBox)
            str = btn.Name
            str = str.Substring(3, 1)
            ind = CInt(str)

            ind1 = GetAsyncKeyState(1)
            ind2 = GetAsyncKeyState(2)

            health = GameChars(ind).Health
            If health <= 0 Then
                'character is death
                Return
            End If


            MyHitsNew = Now()
            If (MyHitsNew - MyHits2(ind)).TotalMilliseconds < MyHitsTempo2 Then
                Return
            Else
                MyHits2(ind) = MyHitsNew
            End If

            If ind2 = 1 Then 'Right Button clicked. Fight Operation
                If FightGoingOn = True Then 'no fight leave
                    weapon = GameChars(ind).Hand2
                    If weapon = ItemSword Or weapon = ItemAxe Or weapon = ItemMace Or weapon = ItemFlail Then
                        If ind > 1 Then 'Swords only 1 row !!!
                            Return
                        End If
                        Hits = Me.DetermineHitpoints()
                        p_effects.BackgroundImage = PictureBox1.Image
                        p_effects.Visible = True
                    ElseIf weapon = ItemBook Then
                        If MySpells(ind) = 0 Then 'no more spells return
                            Return
                        Else
                            MySpells(ind) = MySpells(ind) - 1
                        End If
                        Hits = Me.DetermineHitpointsSpell()
                        p_effects2.BackgroundImage = PictureBox1.Image
                        p_effects2.Visible = True
                    ElseIf weapon = SpellBurningHands Then
                        Hits = Me.DetermineHitpointsSpell()
                        p_effects3.BackgroundImage = PictureBox1.Image
                        p_effects3.Visible = True
                        GameChars(ind).Hand2 = SpellUsed
                        GameChars(ind).Showhand(2)
                        Me.Refresh()
                    ElseIf weapon = SpellFireball Then
                        Hits = Me.DetermineHitpointsSpell()
                        p_effects2.BackgroundImage = PictureBox1.Image
                        p_effects2.Visible = True
                        GameChars(ind).Hand2 = SpellUsed
                        GameChars(ind).Showhand(2)
                        Me.Refresh()


                    End If
                    'Me.ShowMonsterWounds(Hits)
                    TimeEffect.Enabled = True

                    CurrentHealth = CurrentHealth - Hits
                    HEALTHMONSTER.Width = (CurrentHealth / 100) * healthmonster_width
                    If CurrentHealth <= 0 Then
                        p_monster.Visible = False
                        FightGoingOn = False
                        HEALTHMONSTER.Visible = False
                        p_health2.Visible = False
                        TimeMonster.Enabled = False
                        CurrentHealth = 100
                        MyMonsters(PosMonsterX, PosMonsterY) = 0
                        PosMonsterX = 0
                        PosMonsterY = 0
                    End If

                Else
                    'check if this is health potion
                    If GameChars(ind).Hand2 = ItemPotion Then
                        GameChars(ind).Health = MaxHealth
                        GameChars(ind).ShowHealth()
                        GameChars(ind).Hand2 = ItemUsedPotion
                        GameChars(ind).Showhand(2)

                        MsgBox("All health is restored")
                        Return
                    End If

                End If

            ElseIf ind1 = 1 Then 'Left Button clicked. Drag Operation

                If DragDropStarted = False Then
                    SourceDragControl = HAND2PIC(ind)
                    DraggedItem = GameChars(ind).GetHand(2)
                    bmp = Me.Getitem(DraggedItem)
                    Me.NewCursor(bmp, 30, 30)
                    GameChars(ind).UpdateHand(2, 102)
                    GameChars(ind).Showhand(2)
                    DragDropStarted = True
                Else 'Dragdropactive item is dropped here
                    If GameChars(ind).Hand2 <> ItemLeftHand And GameChars(ind).Hand2 <> ItemRightHand Then
                        'drop current weapon
                        Dim CurHand As Integer = GameChars(ind).Hand2
                        MyItemsFloor(CurrentPosX, CurrentPosY) = CurHand
                    End If
                    GameChars(ind).UpdateHand(2, DraggedItem)
                    GameChars(ind).Showhand(2)
                    Me.Cursor = Windows.Forms.Cursors.Arrow
                    DragDropStarted = False
                End If

            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClickHand2")

        End Try


    End Sub

    Private Sub ShowHits(ByVal charnr As Integer, ByVal hits As Integer)

        Dim bmp2 As Bitmap
        Dim bmp1 As New Bitmap(100, 50)

        Try

            bmp1 = HAND1PIC(charnr - 1).Image
            bmp1 = Me.AddSpriteTransBlack(bmp1, HIT, 0, 0)
            Dim gr As Graphics = Graphics.FromImage(bmp1)
            Dim f As New Font("Arial", 10, FontStyle.Bold)
            gr.DrawString(hits.ToString(), f, Brushes.White, 0, 15)
            HAND1PIC(charnr - 1).Image = bmp1

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ShowHits(" + charnr.ToString + "," + hits.ToString + ")")

        End Try

    End Sub

    Private Function GetWeapon(ByVal wnr As Integer) As String
        Dim wtext As String

        Try

            Select Case wnr

                Case 103
                    wtext = " Weapon: Sword"
                Case 104
                    wtext = " Weapon: Axe"
                Case 105
                    wtext = " Weapon: Flail"
                Case 106
                    wtext = " Weapon: Mace"
                Case 110
                    wtext = " Weapon: SpellBook"
                Case 112
                    wtext = " Weapon: Shield"
                Case 202
                    wtext = " Spell: Burning Hands"
                Case 203
                    wtext = " Spell: Fireball"
                Case 205
                    wtext = " Spell: Disintegration"
                Case 206
                    wtext = " Spell: Flesh to Stone"

            End Select

            Return wtext

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "GetWeapon(" + wnr.ToString + ")")


            Return ""

        End Try

    End Function

    Private Sub Hand2Hover(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btn As PictureBox
        Dim str, MeText As String
        Dim ind, ind1, ind2, weapon As Integer

        Try

            btn = CType(sender, PictureBox)
            str = btn.Name
            str = str.Substring(3, 1)
            ind = CInt(str)

            weapon = GameChars(ind).Hand2
            MeText = GetWeapon(weapon)
            Me.Text = "                                                                            " + "Darkmoon RPG" + MeText

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "Hand2Hover")

        End Try

   
    End Sub

    Private Sub Hand1Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try

            Me.Text = "                                                                                       " + "Darkmoon RPG"

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "Hand1Leave")

        End Try

    End Sub


    Private Sub Hand2Leave(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try

            Me.Text = "                                                                                       " + "Darkmoon RPG"

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "Hand2Leave")

        End Try

    End Sub

    Private Sub Hand1Hover(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim btn As PictureBox
        Dim str, MeText As String
        Dim ind, ind1, ind2, weapon As Integer

        Try

            btn = CType(sender, PictureBox)
            str = btn.Name
            str = str.Substring(3, 1)
            ind = CInt(str)

            weapon = GameChars(ind).Hand1
            MeText = GetWeapon(weapon)

            Me.Text = "                                                                            " + "Darkmoon RPG" + MeText

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "Hand1Hover")

        End Try


    End Sub


    Private Sub ClickHand1(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim str As String
        Dim ind, ind1, ind2, health As Integer
        Dim Hits As Integer
        Dim btn As PictureBox
        Dim bmp As Bitmap
        Dim MyHitsNew As Date
        Dim weapon As Integer

        Try

            btn = CType(sender, PictureBox)
            str = btn.Name
            str = str.Substring(3, 1)
            ind = CInt(str)

            health = GameChars(ind).Health
            If health <= 0 Then
                'character is death
                Return
            End If

            MyHitsNew = Now()
            If (MyHitsNew - MyHits1(ind)).TotalMilliseconds < MyHitsTempo1 Then
                Return
            Else
                MyHits1(ind) = MyHitsNew
            End If


            ind1 = GetAsyncKeyState(1)
            ind2 = GetAsyncKeyState(2)


            'MsgBox((MyHits - MyHitsNew).TotalMilliseconds)

            If ind2 = 1 Then 'Right Button clicked. Fight Operation

                If FightGoingOn = True Then 'no fight leave
                    weapon = GameChars(ind).Hand1
                    If weapon = ItemSword Or weapon = ItemAxe Or weapon = ItemMace Or weapon = ItemFlail Then
                        If ind > 1 Then 'Swords only 1 row !!!
                            Return
                        End If
                        Hits = Me.DetermineHitpoints()
                        p_effects.BackgroundImage = PictureBox1.Image
                        p_effects.Visible = True
                    ElseIf weapon = ItemBook Then
                        If MySpells(ind) = 0 Then 'no more spells return
                            Return
                        Else
                            MySpells(ind) = MySpells(ind) - 1
                        End If
                        Hits = Me.DetermineHitpointsSpell()
                        p_effects2.BackgroundImage = PictureBox1.Image
                        p_effects2.Visible = True

                    ElseIf weapon = SpellFireball Then
                        Hits = Me.DetermineHitpointsSpell()
                        p_effects2.BackgroundImage = PictureBox1.Image
                        p_effects2.Visible = True
                        GameChars(ind).Hand1 = SpellUsed
                        GameChars(ind).Showhand(1)
                        Me.Refresh()

                    ElseIf weapon = SpellBurningHands Then
                        Hits = Me.DetermineHitpointsSpell()
                        p_effects3.BackgroundImage = PictureBox1.Image
                        p_effects3.Visible = True
                        GameChars(ind).Hand1 = SpellUsed
                        GameChars(ind).Showhand(1)
                        Me.Refresh()

                    End If
                    CurrentHealth = CurrentHealth - Hits
                    TimeEffect.Enabled = True

                    'Show new Health of monster
                    HEALTHMONSTER.Width = (CurrentHealth / 100) * healthmonster_width

                    'if health = 0 then. remove monster. Fightgoingon = false
                    If CurrentHealth <= 0 Then
                        p_monster.Visible = False
                        FightGoingOn = False
                        HEALTHMONSTER.Visible = False
                        p_health2.Visible = False
                        TimeMonster.Enabled = False
                        CurrentHealth = 100
                        MyMonsters(PosMonsterX, PosMonsterY) = 0
                        PosMonsterX = 0
                        PosMonsterY = 0

                    End If

                Else

                    'check if this is health potion
                    If GameChars(ind).Hand1 = ItemPotion Then
                        GameChars(ind).Health = MaxHealth
                        GameChars(ind).ShowHealth()
                        GameChars(ind).Hand1 = ItemUsedPotion
                        GameChars(ind).Showhand(1)

                        MsgBox("All health is restored")
                        Return
                    End If


                End If
            Else
                If ind1 = 1 Then 'Left Button clicked. Drag Operation
                    If DragDropStarted = False Then
                        SourceDragControl = HAND1PIC(ind)
                        DraggedItem = GameChars(ind).GetHand(1)
                        bmp = Me.Getitem(DraggedItem)
                        Me.NewCursor(bmp, 30, 30)
                        GameChars(ind).UpdateHand(1, 101) '41)
                        GameChars(ind).Showhand(1)
                        'HAND1PIC(ind).Image = SWORD
                        DragDropStarted = True
                    Else 'Dragdropactive item is dropped here
                        If GameChars(ind).Hand1 <> ItemLeftHand And GameChars(ind).Hand1 <> ItemRightHand Then
                            'drop current weapon
                            Dim CurHand As Integer = GameChars(ind).Hand1
                            MyItemsFloor(CurrentPosX, CurrentPosY) = CurHand

                        End If

                        GameChars(ind).UpdateHand(1, DraggedItem)
                        GameChars(ind).Showhand(1)
                        Me.Cursor = Windows.Forms.Cursors.Arrow
                        DragDropStarted = False
                    End If

                End If

            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "ClickHand1")

        End Try

    End Sub

    Private Function DetermineWoundedCharacter() As Integer

        Dim hitpoints As Integer

        Try

            hitpoints = CInt(Math.Ceiling(Rnd() * 2))
            If hitpoints <= 1 Then
                Return 1
            Else
                Return 2
            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "DetermineWoundedCharacter")

        End Try

    End Function

    Private Function DetermineHitpointsMonster() As Integer

        Dim hitpoints As Integer

        Try

            'number between 5 and 8
            hitpoints = CInt(Math.Floor((MonsterHitpoints - 5 + 1) * Rnd())) + 5

            Return hitpoints

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "DetermineHitpointsMonster")
            Return 0


        End Try

    End Function

    Private Function DetermineHitpoints() As Integer
        Dim hitpoints As Integer

        Try

            'number between 5 and 10
            hitpoints = CInt(Math.Floor((SwordHitpoints - 5 + 1) * Rnd())) + 5
            Return hitpoints

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "DetermineHitpoints")
            Return 0

        End Try

    End Function

    Private Function DetermineHitpointsSpell() As Integer
        Dim hitpoints As Integer

        Try

            'number between 5 and 12
            hitpoints = CInt(Math.Floor((BookHitpoints - 5 + 1) * Rnd())) + 5

            Return hitpoints

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "DetermineHitpointsSpell")
            Return 0

        End Try

    End Function


    Private Sub TimeEffect_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeEffect.Tick

        Try

            p_effects.Visible = False
            p_effects2.Visible = False
            p_effects3.Visible = False
            TimeEffect.Enabled = False

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "TimeEffect_Tick")

        End Try

    End Sub

    Public Function BlurPict(ByVal blurcount As Single, ByVal blurimg As Bitmap) As Bitmap
        Dim m_Alpha As Single = blurcount ' Alpha on a 0-1 scale.
        Dim m_DAlpha As Single = blurcount

        Dim bm1 As Bitmap = blurimg
        Dim bm As New Bitmap(bm1.Width, bm1.Height)
        Dim gr As Graphics = Graphics.FromImage(bm)

        Try

            Dim image_attr As New System.Drawing.Imaging.ImageAttributes
            Dim cm As System.Drawing.Imaging.ColorMatrix
            Dim rect As Rectangle = Rectangle.Round(bm1.GetBounds(GraphicsUnit.Pixel))

            cm = New System.Drawing.Imaging.ColorMatrix(New Single()() { _
                New Single() {1.0, 0.0, 0.0, 0.0, 0.0}, _
                New Single() {0.0, 1.0, 0.0, 0.0, 0.0}, _
                New Single() {0.0, 0.0, 1.0, 0.0, 0.0}, _
                New Single() {0.0, 0.0, 0.0, 0.0, 0.0}, _
                New Single() {0.0, 0.0, 0.0, m_Alpha, 1.0}})
            image_attr.SetColorMatrix(cm)

            gr.DrawImage(bm1, rect, 0, 0, bm1.Width, bm1.Height, GraphicsUnit.Pixel, image_attr)
            'PB1.Image = bm
            Return bm

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "BlurPict")
            Return bm

        End Try

    End Function


    Public Function Blur(ByVal blurcount As Single, ByVal charnr As Integer) As Boolean
        Dim PB1 As PictureBox

        Try

            PB1 = PBPortrait(charnr - 1)

            Dim m_Alpha As Single = blurcount ' Alpha on a 0-1 scale.
            Dim m_DAlpha As Single = blurcount

            Dim bm1 As Bitmap = PB1.Image.Clone

            Dim bm As New Bitmap(bm1.Width, bm1.Height)
            Dim gr As Graphics = Graphics.FromImage(bm)

            Dim image_attr As New System.Drawing.Imaging.ImageAttributes
            Dim cm As System.Drawing.Imaging.ColorMatrix
            Dim rect As Rectangle = Rectangle.Round(bm1.GetBounds(GraphicsUnit.Pixel))

            cm = New System.Drawing.Imaging.ColorMatrix(New Single()() { _
                New Single() {1.0, 0.0, 0.0, 0.0, 0.0}, _
                New Single() {0.0, 1.0, 0.0, 0.0, 0.0}, _
                New Single() {0.0, 0.0, 1.0, 0.0, 0.0}, _
                New Single() {0.0, 0.0, 0.0, 0.0, 0.0}, _
                New Single() {0.0, 0.0, 0.0, m_Alpha, 1.0}})
            image_attr.SetColorMatrix(cm)

            gr.DrawImage(bm1, rect, 0, 0, bm1.Width, bm1.Height, GraphicsUnit.Pixel, image_attr)
            PBPortrait(charnr - 1).Image = bm

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "Blur(" + charnr.ToString + ")")

        End Try

    End Function


    Private Sub TimeCharacter_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeCharacter.Tick

        Try

            Me.Blur(1, 1)
            Me.Blur(1, 2)
            TimeCharacter.Enabled = False

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "TimeCharacter_Tick")

        End Try


    End Sub

    Private Sub TimeBitmap_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeBitmap.Tick

        Try

            TimeBitmap.Enabled = False
            TimeBitmap2.Enabled = True

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "TimeBitmap_Tick")

        End Try

    End Sub


    Private Sub bCast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bCast.Click

        Dim hits As Integer

        Try

            If FightGoingOn = True Then

                hits = Me.DetermineHitpoints()
                CurrentHealth = CurrentHealth - hits
                p_effects.BackgroundImage = PictureBox1.Image
                p_effects.Visible = True
                TimeEffect.Enabled = True
                HEALTHMONSTER.Width = (CurrentHealth / 100) * healthmonster_width

                If CurrentHealth <= 0 Then
                    p_monster.Visible = False
                    FightGoingOn = False
                    HEALTHMONSTER.Visible = False
                    p_health2.Visible = False
                    TimeMonster.Enabled = False
                    CurrentHealth = 100

                End If

            End If

        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "bCast_Click")

        End Try

    End Sub


    Private Sub TimeBitmap2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeBitmap2.Tick
        Dim hits, thechar As Integer

        Try

            TimeBitmap2.Enabled = False
            'which character is hit ?
            thechar = Me.DetermineWoundedCharacter()
            'determine hitpoints
            hits = Me.DetermineHitpointsMonster()
            If GameChars(thechar - 1).Hand1 = ItemShield Or GameChars(thechar - 1).Hand2 = ItemShield Then
                hits = CInt(hits * ProtectionShield)
            End If

            Me.Blur(0.3, thechar)
            TimeCharacter.Enabled = True

            GameChars(thechar - 1).UpdateHealth(hits)
            If GameChars(thechar - 1).Health <= 0 Then
                PBPortrait(thechar - 1).Image = Me.LoadDeath(1)
                'check if char behind can switch ???
                If GameChars(thechar + 1).Health > 0 Then
                    Me.FlipControlsNew(thechar, thechar + 2)
                End If
            End If
            If GameChars(0).Health <= 0 And GameChars(1).Health <= 0 And GameChars(2).Health <= 0 And GameChars(3).Health <= 0 Then
                TimeMonster.Enabled = False
                TimeBitmap.Enabled = False
                TimeCharacter.Enabled = False
                MsgBox("You're whole party has been killed ! You have to start a new game !")
                Me.Close()

            End If


        Catch ex As Exception

            MsgBox(ex.Message)
            LogMessage(ex.Message, "TimeBitmap2_Tick")

        End Try

    End Sub



    Private Sub T04_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles T04.Click

        'FightGoingOn = False
        'HEALTHMONSTER.Visible = False
        'p_monster.Visible = False
        'p_health2.Visible = False
        'TimeMonster.Enabled = False
        'CurrentHealth = 100

        'T01.Visible = False
        'T02.Visible = False
        'T03.Visible = False
        'T04.Visible = False
        'cbWalls.Visible = False

    End Sub

    Private Sub PictureBox5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox5.Click

        'cbWalls.Visible = True

    End Sub

    Private Sub cbWalls_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbWalls.SelectedIndexChanged

        'Select Case cbWalls.Text

        '    Case "Silver"
        '        BackdropId = 1
        '        WallsId = 1
        '        StairsId = 1
        '        DoorId = 1
        '    Case "Temple"
        '        BackdropId = 2
        '        WallsId = 2
        '        StairsId = 2
        '        DoorId = 2
        '    Case "Catacombs"
        '        BackdropId = 3
        '        WallsId = 3
        '        StairsId = 3
        '        DoorId = 3
        '    Case "Wood"
        '        BackdropId = 4
        '        WallsId = 4
        '        StairsId = 4
        '        DoorId = 4
        '    Case "Sewers"
        '        BackdropId = 5
        '        WallsId = 5
        '        StairsId = 5
        '        DoorId = 5
        '    Case "Dwarven"
        '        BackdropId = 6
        '        WallsId = 6
        '        StairsId = 6
        '        DoorId = 6
        '    Case "Drow"
        '        BackdropId = 7
        '        WallsId = 7
        '        StairsId = 7
        '        DoorId = 7
        '    Case "Sanctum"
        '        BackdropId = 8
        '        WallsId = 8
        '        StairsId = 8
        '        DoorId = 8
        '    Case "Green"
        '        BackdropId = 9
        '        WallsId = 9
        '        StairsId = 9
        '        DoorId = 9
        '    Case "Crimson"
        '        BackdropId = 10
        '        WallsId = 10
        '        StairsId = 10
        '        DoorId = 10
        '    Case "Mage"
        '        BackdropId = 11
        '        WallsId = 11
        '        StairsId = 11
        '        DoorId = 11

        'End Select


        'Me.InitializeBitmaps()
        'Me.PaintWalls()

        ''Me.LoadDatasets()
        ''Me.FillGameFieldControls()
        ''Me.FillMapsandItems()


        'Me.Refresh()

    End Sub

    Private Sub PictureBox2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox2.Click

        'If BigMap = False Then
        '    PictureBox6.Visible = True
        '    PictureBox7.Visible = True
        '    BigMap = True
        'Else
        '    PictureBox6.Visible = False
        '    PictureBox7.Visible = False
        '    BigMap = False

        'End If


    End Sub

    Private Sub PictureBox6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox6.Click

        'If BigMap = False Then
        '    PictureBox6.Visible = True
        '    PictureBox7.Visible = True
        '    BigMap = True
        'Else
        '    PictureBox6.Visible = False
        '    PictureBox7.Visible = False
        '    BigMap = False

        'End If

    End Sub

End Class
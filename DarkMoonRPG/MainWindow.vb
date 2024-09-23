Imports System.IO
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.Common

Public Class MainWindow
    Private Declare Unicode Function LoadCursorFromFile Lib "user32.dll" Alias "LoadCursorFromFileW" (ByVal filename As String) As IntPtr
    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vKey As Long) As Integer

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.PositionControls()

        CurrentPosX3 = CurrentPosX + 3
        CurrentPosY3 = CurrentPosY + 3

        p_monster.SizeMode = PictureBoxSizeMode.StretchImage
        Me.LoadDatasets()

        GameFile = CurDir()
        Me.InitializeBitmaps()
        Me.FillMapsDB(CurrentLevel)

        Dim bmp As Bitmap = Bitmap.FromFile(GameFile + "\picts\EOB-BACKDROP-1.bmp")
        PictureBox1.Image = bmp
        PictureBox1.Width = MainWindow_width
        PictureBox1.Height = MainWindow_height

        Me.PaintWalls()
        Me.ShowCounters()

        PRO01.ForeColor = Color.Green
        PRO01.Text = "Health"
        'PRO02.ForeColor = Color.Blue
        'PRO02.Text = "Health"
        'char1 = New Character(Me, "Reiniere", "M", "Human", "Fighter", 1, 1)
        'char2 = New Character(Me, "Sallie", "M", "Human", "Fighter", 2, 2)
        'char3 = New Character(Me, "Dorian", "M", "Human", "Fighter", 3, 3)
        'char4 = New Character(Me, "Antonia", "M", "Human", "Fighter", 4, 4)

        'Me.Cursor = Cursors.Arrow
        'Dim curfile As String = Application.StartupPath & "\cursors\Big1.cur"

        'Dim hcur As IntPtr
        'hcur = LoadCursorFromFile(curfile)
        'Me.Cursor = New Cursor(hcur)

        'Me.NewCursor(SWORD, 30, 30)

        'Me.Cursor = New Cursor(My.Resources.MainWindow.sword.Handle)
        'Me.Cursor = New Cursor(My.Resources.MainWindow.sword2.Handle)



    End Sub

    Public Sub PositionControls()
        P001.Location = New Point(H01.Location.X, H01.Location.Y)
        H01.BringToFront()
        P002.Location = New Point(H02.Location.X, H02.Location.Y)
        H02.BringToFront()
        P003.Location = New Point(H03.Location.X, H03.Location.Y)
        H03.BringToFront()
        P004.Location = New Point(H04.Location.X, H04.Location.Y)
        H04.BringToFront()

        P005.Location = New Point(M01.Location.X, M01.Location.Y)
        M01.BringToFront()
        P006.Location = New Point(M02.Location.X, M02.Location.Y)
        M02.BringToFront()
        P007.Location = New Point(M03.Location.X, M03.Location.Y)
        M03.BringToFront()
        P008.Location = New Point(M04.Location.X, M04.Location.Y)
        M04.BringToFront()

        p_health2.Location = New Point(p_health.Location.X, p_health.Location.Y)


    End Sub


    Public Sub NewCursor(ByVal bmp As Bitmap, ByVal w As Integer, ByVal h As Integer)

        Dim img As New Bitmap(w, h)
        img = bmp
        img.MakeTransparent(Color.Black)
        Me.Cursor = create.CreateCursor(SWORD, 30, 30)

    End Sub

    Public Sub LoadDatasets()

        Me.LoadTreps()
        Me.LoadPortals()
        Me.LoadDoors()

    End Sub
    Public Sub LoadDoors()
        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection

        dsDoors.Reset()
        mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
        myconnection1 = New OleDbConnection(mystring)

        da = New OleDbDataAdapter
        mysql = "select * from Doors"
        da.SelectCommand = New OleDbCommand(mysql, myconnection1)

        myconnection1.Open()

        da.Fill(dsDoors, "Doors")


    End Sub

    Public Sub LoadPortals()
        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection

        dsTreps.Reset()
        mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
        myconnection1 = New OleDbConnection(mystring)

        da = New OleDbDataAdapter
        mysql = "select * from Portals"
        da.SelectCommand = New OleDbCommand(mysql, myconnection1)

        myconnection1.Open()

        da.Fill(dsPortals, "Portals")
        'str = dsPortals.Tables(0).Rows(0).Item(5).ToString() + "," + dsPortals.Tables(0).Rows(0).Item(6).ToString() + "," + dsPortals.Tables(0).Rows(0).Item(7).ToString()
        'MsgBox(str)

    End Sub

    Public Sub LoadTreps()
        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection

        dsTreps.Reset()
        mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
        myconnection1 = New OleDbConnection(mystring)

        da = New OleDbDataAdapter
        mysql = "select * from Treps"
        da.SelectCommand = New OleDbCommand(mysql, myconnection1)

        myconnection1.Open()



        da.Fill(dsTreps, "Treps")
        'str = dsTreps.Tables(0).Rows(0).Item(5).ToString() + "," + dsTreps.Tables(0).Rows(0).Item(6).ToString() + "," + dsTreps.Tables(0).Rows(0).Item(7).ToString()
        'MsgBox(str)


    End Sub


    Public Sub PaintWalls()

        Select Case CurrentView

            Case 0 'North
                Me.PaintWallsNorth()

            Case 1 'East
                Me.PaintWallsEast()

            Case 2 'South
                Me.PaintWallsSouth()

            Case 3 'West
                Me.PaintWallsWest()

        End Select
        Me.ShowCounters()

    End Sub


    Public Sub PaintItemL1Copy(ByRef bmp As Bitmap, ByVal item As Integer, ByVal pos As Integer, ByVal px As Integer, ByVal py As Integer)
        Dim x, y As Integer

        x = px + MainWindow_width
        y = py
        Select Case item
            Case 0 'wall
                Select Case pos
                    Case 1
                        'bmp = Me.AddSprite(bmp, WALL_L01_left, x, y)
                        bmp = Me.AddSprite(bmp, WALL_L01, x, y)
                    Case 2
                        bmp = Me.AddSprite(bmp, WALL_L01, x, y)
                    Case 3
                        'bmp = Me.AddSprite(bmp, WALL_L01_right, x, y)
                        bmp = Me.AddSprite(bmp, WALL_L01, x, y)

                End Select

            Case 5 'trep
                Select Case pos
                    Case 1
                    Case 2
                        bmp = Me.AddSprite(bmp, STAIRS01A, x, y)
                    Case 3

                End Select

        End Select

    End Sub

    Public Sub PaintItemL3(ByRef bmp As Bitmap, ByVal item As Integer, ByVal pos As Integer, ByVal px As Integer, ByVal py As Integer)

        Dim x, y As Integer

        x = px + MainWindow_width
        y = py

        Select Case pos

            Case 0

                Select Case item
                    Case 0
                        bmp = Me.AddSprite(bmp, WALL_L03, x, y)
                    Case 5

                End Select

            Case 1

                Select Case item
                    Case 0
                        bmp = Me.AddSprite(bmp, WALL_L03, x, y)
                    Case 5
                        bmp = Me.AddSprite(bmp, STAIRS03A, x, y)
                    Case 6
                        bmp = Me.AddSprite(bmp, STAIRS03B, x, y)


                End Select
            Case 2
                Select Case item
                    Case 0
                        bmp = Me.AddSprite(bmp, WALL_L03, x, y)
                    Case 5
                        bmp = Me.AddSprite(bmp, STAIRS03A, x, y)
                    Case 6
                        bmp = Me.AddSprite(bmp, STAIRS03B, x, y)


                End Select

            Case 3
                Select Case item
                    Case 0
                        bmp = Me.AddSprite(bmp, WALL_L03, x, y)
                    Case 5
                        bmp = Me.AddSprite(bmp, STAIRS03A, x, y)
                    Case 6
                        bmp = Me.AddSprite(bmp, STAIRS03B, x, y)


                End Select

            Case 4

                Select Case item
                    Case 0
                        bmp = Me.AddSprite(bmp, WALL_L03, x, y)
                    Case 5

                End Select

        End Select

    End Sub

    Public Sub PaintItemL2(ByRef bmp As Bitmap, ByVal item As Integer, ByVal pos As Integer, ByVal px As Integer, ByVal py As Integer)

        Dim x, y As Integer

        x = px + MainWindow_width
        y = py

        Select Case pos

            Case 0
                Select Case item
                    Case 0
                        bmp = Me.AddSpriteTransBlack(bmp, SWLEFT_L03, x, y)
                    Case 5
                End Select

            Case 1

                Select Case item
                    Case 0
                        bmp = Me.AddSprite(bmp, WALL_L02, x, y)
                    Case 5
                        bmp = Me.AddSprite(bmp, STAIRS02A, x, y)
                    Case 6
                        bmp = Me.AddSprite(bmp, STAIRS02B, x, y)
  

                End Select
            Case 2
                Select Case item
                    Case 0
                        bmp = Me.AddSprite(bmp, WALL_L02, x, y)
                    Case 5
                        bmp = Me.AddSprite(bmp, STAIRS02A, x, y)
                    Case 6
                        bmp = Me.AddSprite(bmp, STAIRS02B, x, y)


                End Select

            Case 3
                Select Case item
                    Case 0
                        bmp = Me.AddSprite(bmp, WALL_L02, x, y)
                    Case 5
                        bmp = Me.AddSprite(bmp, STAIRS02A, x, y)
                    Case 6
                        bmp = Me.AddSprite(bmp, STAIRS02B, x, y)


                End Select

            Case 4
                Select Case item
                    Case 0
                        bmp = Me.AddSpriteTransBlack(bmp, SWRIGHT_L03, x, y)
                    Case 5
                End Select

        End Select

    End Sub


    Public Sub PaintItemL1(ByRef bmp As Bitmap, ByVal item As Integer, ByVal pos As Integer, ByVal px As Integer, ByVal py As Integer)
        Dim x, y As Integer

        x = px + MainWindow_width
        y = py

        Select Case pos

            Case 0
                Select Case item
                    Case 0
                        bmp = Me.AddSpriteTransBlack(bmp, SWLEFT_L02, x, y)
                    Case 5
                End Select

            Case 1

                Select Case item
                    Case 0
                        bmp = Me.AddSprite(bmp, WALL_L01, x, y)
                    Case 5
                        bmp = Me.AddSprite(bmp, STAIRS01A, x, y)
                    Case 6
                        bmp = Me.AddSprite(bmp, STAIRS01B, x, y)



                End Select
            Case 2
                Select Case item
                    Case 0
                        bmp = Me.AddSprite(bmp, WALL_L01, x, y)
                        'bmp = Me.AddSpriteTransWhite(bmp, KEYHOLE01, x + (WALL_L01.Width / 2.1), y + (WALL_L01.Height / 2.5))
                        'bmp = Me.AddSpriteTransWhite(bmp, TELEPORT, x, y)

                    Case 5
                        bmp = Me.AddSprite(bmp, STAIRS01A, x, y)
                    Case 6
                        bmp = Me.AddSprite(bmp, STAIRS01B, x, y)
                    Case 7 'portal
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



                End Select

            Case 4
                Select Case item
                    Case 0
                        bmp = Me.AddSpriteTransBlack(bmp, SWRIGHT_L02, x, y)
                    Case 5
                End Select

            Case 5 'level 0 sidewall
                Select Case item
                    Case 0
                        bmp = Me.AddSpriteTransBlack(bmp, SWLEFT_L01, x, y)
                    Case 5
                End Select

            Case 6 'level 0 sidewall
                Select Case item
                    Case 0
                        bmp = Me.AddSpriteTransBlack(bmp, SWRIGHT_L01, x, y)
                    Case 5
                End Select
        End Select

    End Sub

    Public Sub CopyToBigGameMap(ByRef srcbmp As Bitmap, ByVal trgbmp As Bitmap)
        Dim mycolor As Color
        Dim xpos, ypos As Integer

        xpos = MainWindow_width
        ypos = 0
        For x = 0 To MainWindow_width - 1
            For y = 0 To MainWindow_height - 1
                mycolor = srcbmp.GetPixel(x, y)
                trgbmp.SetPixel(xpos + x, ypos + y, mycolor)
            Next
            'xpos = xpos + 1
        Next

    End Sub

    Public Sub CopyFromBigGameMap(ByRef srcbmp As Bitmap, ByVal trgbmp As Bitmap)
        Dim mycolor As Color
        Dim xpos, ypos As Integer

        xpos = MainWindow_width
        ypos = 0
        For x = 0 To MainWindow_width - 1
            For y = 0 To MainWindow_height - 1
                mycolor = trgbmp.GetPixel(x + xpos, y + ypos)
                srcbmp.SetPixel(x, y, mycolor)
            Next
            'xpos = xpos + 1
        Next
    End Sub

    Public Sub PaintTheWalls(ByVal px As Integer, ByVal py As Integer, ByVal dir As String)
        Dim mainbmp As New Bitmap(MainWindow_width, MainWindow_height)
        Dim mainbmp2 As New Bitmap(MainWindow_width, MainWindow_height)
        Dim mainbmpbig As New Bitmap(MainWindow_width * 3, MainWindow_height)

        ' X7 X8 X9
        ' X4 X5 X6
        ' X1 X2 X3
        '  X O X (kunnen alleen sidewalls zijn)


        'mainbmp = GameField
        mainbmp = Bitmap.FromFile(GameFile + "\picts\EOB-BACKDROP-1.bmp")
        Me.CopyToBigGameMap(mainbmp, mainbmpbig)

        'check position p12 (in front)
        If p12 <> 1 Then
            Me.PaintItemL1(mainbmpbig, p12, 2, MW_lvl1_xpos, MW_lvl1_ypos)
            'mainbmp = Me.AddSprite(mainbmp, WALL_L01, MW_lvl1_xpos, MW_lvl1_ypos)
            If p10 <> 1 Then
                Me.PaintItemL1(mainbmpbig, p10, 5, SWL_lvl1_xpos, SWL_lvl1_ypos)
                'mainbmpbig = Me.AddSprite(mainbmpbig, SWLEFT_L01, MainWindow_width + SWL_lvl1_xpos, SWL_lvl1_ypos)
            Else
                If p11 <> 1 Then 'draw second lvl1 wall left
                    Me.PaintItemL1(mainbmpbig, p11, 1, MW_lvl1_xpos - WALL_L01.Width, MW_lvl1_ypos)
                    'mainbmp = Me.AddSprite(mainbmp, WALL_L01_left, 0, MW_lvl1_ypos)
                End If
            End If
            If p01 <> 1 Then
                Me.PaintItemL1(mainbmpbig, p01, 6, SWR_lvl1_xpos, SWR_lvl1_ypos)
                'mainbmpbig = Me.AddSprite(mainbmpbig, SWRIGHT_L01, MainWindow_width + SWR_lvl1_xpos, SWR_lvl1_ypos)
            Else
                If p13 <> 1 Then 'draw second lvl1 wall left
                    Me.PaintItemL1(mainbmpbig, p13, 3, MW_lvl1_xpos + WALL_L01.Width, MW_lvl1_ypos)
                    'mainbmp = Me.AddSprite(mainbmp, WALL_L01_right, WALL_L01.Width + WALL_L01_left.Width - 2, MW_lvl1_ypos)
                End If
            End If
        Else
            'check position p22 (2-nd row)
            If p22 <> 1 Then
                Me.PaintItemL2(mainbmpbig, p22, 2, MW_lvl2_xpos, MW_lvl2_ypos)
                'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L02, MainWindow_width + MW_lvl2_xpos, MW_lvl2_ypos)
                If p11 <> 1 Then
                    Me.PaintItemL1(mainbmpbig, p11, 0, SWL_lvl2_xpos, SWL_lvl2_ypos)
                    'mainbmpbig = Me.AddSprite(mainbmpbig, SWLEFT_L02, MainWindow_width + SWL_lvl2_xpos, SWL_lvl2_ypos)
                Else
                    If p21 <> 1 Then
                        Me.PaintItemL2(mainbmpbig, p21, 1, MW_lvl2_xpos - WALL_L02.Width, MW_lvl2_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L02_left, MainWindow_width, MW_lvl2_ypos)
                    End If
                End If
                If p13 <> 1 Then
                    Me.PaintItemL1(mainbmpbig, p13, 4, SWR_lvl2_xpos, SWR_lvl2_ypos)
                    'mainbmpbig = Me.AddSprite(mainbmpbig, SWRIGHT_L02, MainWindow_width + SWR_lvl2_xpos, SWR_lvl2_ypos)
                Else
                    If p23 <> 1 Then
                        Me.PaintItemL2(mainbmpbig, p23, 3, MW_lvl2_xpos + WALL_L02.Width, MW_lvl2_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L02_right, MainWindow_width + WALL_L02.Width + WALL_L02_left.Width - 2, MW_lvl2_ypos)
                    End If
                End If
                If p10 <> 1 Then
                    Me.PaintItemL1(mainbmpbig, p10, 5, SWL_lvl1_xpos, SWL_lvl1_ypos)
                    'mainbmpbig = Me.AddSprite(mainbmpbig, SWLEFT_L01, MainWindow_width + SWL_lvl1_xpos, SWL_lvl1_ypos)
                Else
                    If p11 <> 1 Then 'draw second lvl1 wall left
                        Me.PaintItemL1(mainbmpbig, p11, 1, MW_lvl1_xpos - WALL_L01.Width, MW_lvl1_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L01_left, MainWindow_width, MW_lvl1_ypos)
                    End If
                End If
                If p01 <> 1 Then
                    Me.PaintItemL1(mainbmpbig, p01, 6, SWR_lvl1_xpos, SWR_lvl1_ypos)
                    'mainbmpbig = Me.AddSprite(mainbmpbig, SWRIGHT_L01, MainWindow_width + SWR_lvl1_xpos, SWR_lvl1_ypos)
                Else
                    If p13 <> 1 Then 'draw second lvl1 wall left
                        Me.PaintItemL1(mainbmpbig, p13, 3, MW_lvl1_xpos + WALL_L01.Width, MW_lvl1_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L01_right, MainWindow_width + WALL_L01.Width + WALL_L01_left.Width - 2, MW_lvl1_ypos)
                    End If
                End If
            Else
                'check position p32 (3-nd row)
                If p32 <> 1 Then
                    Me.PaintItemL3(mainbmpbig, p32, 2, MW_lvl3_xpos, MW_lvl3_ypos)
                    'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L03, MainWindow_width + MW_lvl3_xpos, MW_lvl3_ypos)
                    If p21 <> 1 Then
                        Me.PaintItemL2(mainbmpbig, p21, 0, SWL_lvl3_xpos, SWL_lvl3_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, SWLEFT_L03, MainWindow_width + SWL_lvl3_xpos, SWL_lvl3_ypos)
                        Me.PaintItemL2(mainbmpbig, p21, 1, MW_lvl2_xpos - WALL_L02.Width, MW_lvl2_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L02_left, MainWindow_width, MW_lvl2_ypos)
                    Else
                        If p30 <> 1 Then
                            Me.PaintItemL3(mainbmpbig, p30, 0, MW_lvl3_xpos - WALL_L03.Width - WALL_L03.Width, MW_lvl3_ypos)
                            'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L03_left, MainWindow_width, MW_lvl3_ypos)
                        End If
                        If p31 <> 1 Then
                            Me.PaintItemL3(mainbmpbig, p31, 1, MW_lvl3_xpos - WALL_L03.Width, MW_lvl3_ypos)
                            'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L03, MainWindow_width + MW_lvl3_xpos - WALL_L03.Width + 1, MW_lvl3_ypos)
                        End If

                    End If
                    If p23 <> 1 Then
                        Me.PaintItemL2(mainbmpbig, p23, 4, SWR_lvl3_xpos, SWR_lvl3_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, SWRIGHT_L03, MainWindow_width + SWR_lvl3_xpos, SWR_lvl3_ypos)
                        Me.PaintItemL2(mainbmpbig, p23, 3, MW_lvl2_xpos + WALL_L02.Width, MW_lvl2_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L02_right, MainWindow_width + WALL_L02.Width + WALL_L02_left.Width - 2, MW_lvl2_ypos)
                    Else
                        If p34 <> 1 Then
                            Me.PaintItemL3(mainbmpbig, p34, 4, MW_lvl3_xpos + WALL_L03.Width + WALL_L03.Width, MW_lvl3_ypos)
                            'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L03_right, MainWindow_width + MW_lvl3_xpos + WALL_L03.Width + WALL_L03.Width - 2, MW_lvl3_ypos)
                        End If
                        If p33 <> 1 Then
                            Me.PaintItemL3(mainbmpbig, p33, 3, MW_lvl3_xpos + WALL_L03.Width, MW_lvl3_ypos)
                            'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L03, MainWindow_width + MW_lvl3_xpos + WALL_L03.Width - 1, MW_lvl3_ypos)
                        End If

                    End If
                    If p11 <> 1 Then
                        Me.PaintItemL1(mainbmpbig, p11, 0, SWL_lvl2_xpos, SWL_lvl2_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, SWLEFT_L02, MainWindow_width + SWL_lvl2_xpos, SWL_lvl2_ypos)
                    End If
                    If p13 <> 1 Then
                        Me.PaintItemL1(mainbmpbig, p13, 4, SWR_lvl2_xpos, SWR_lvl2_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, SWRIGHT_L02, MainWindow_width + SWR_lvl2_xpos, SWR_lvl2_ypos)
                    End If
                    If p10 <> 1 Then
                        Me.PaintItemL1(mainbmpbig, p10, 5, SWL_lvl1_xpos, SWL_lvl1_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, SWLEFT_L01, MainWindow_width + SWL_lvl1_xpos, SWL_lvl1_ypos)
                    Else
                        If p11 <> 1 Then 'draw second lvl1 wall left
                            Me.PaintItemL1(mainbmpbig, p11, 1, MW_lvl1_xpos - WALL_L01.Width, MW_lvl1_ypos)
                            'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L01_left, MainWindow_width, MW_lvl1_ypos)
                        End If
                    End If
                    If p01 <> 1 Then
                        Me.PaintItemL1(mainbmpbig, p01, 6, SWR_lvl1_xpos, SWR_lvl1_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, SWRIGHT_L01, MainWindow_width + SWR_lvl1_xpos, SWR_lvl1_ypos)
                    Else
                        If p13 <> 1 Then 'draw second lvl1 wall left
                            Me.PaintItemL1(mainbmpbig, p13, 3, MW_lvl1_xpos + WALL_L01.Width, MW_lvl1_ypos)
                            'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L01_right, MainWindow_width + WALL_L01.Width + WALL_L01_left.Width - 2, MW_lvl1_ypos)
                        End If
                    End If
                Else
                    If p30 <> 1 Then
                        'KLOPT NIET ! MOET LVL4 SIDEWALL ZIJN
                        'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L03_left, MainWindow_width, MW_lvl3_ypos)
                    End If
                    If p31 <> 1 Then
                        Me.PaintItemL3(mainbmpbig, p31, 1, MW_lvl3_xpos - WALL_L03.Width, MW_lvl3_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L03, MainWindow_width + MW_lvl3_xpos - WALL_L03.Width + 1, MW_lvl3_ypos)

                    End If
                    If p33 <> 1 Then
                        Me.PaintItemL3(mainbmpbig, p33, 3, MW_lvl3_xpos + WALL_L03.Width, MW_lvl3_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L03, MainWindow_width + MW_lvl3_xpos + WALL_L03.Width - 1, MW_lvl3_ypos)

                    End If
                    If p34 <> 1 Then
                        'KLOPT NIET ! MOET LVL4 SIDEWALL ZIJN
                        'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L03_right, MainWindow_width + MW_lvl3_xpos + WALL_L03.Width + WALL_L03.Width - 2, MW_lvl3_ypos)
                    End If
                    If p21 <> 1 Then
                        Me.PaintItemL2(mainbmpbig, p21, 0, SWL_lvl3_xpos, SWL_lvl3_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, SWLEFT_L03, MainWindow_width + SWL_lvl3_xpos, SWL_lvl3_ypos)
                        Me.PaintItemL2(mainbmpbig, p21, 1, MW_lvl2_xpos - WALL_L02.Width, MW_lvl2_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L02_left, MainWindow_width, MW_lvl2_ypos)
                    End If
                    If p23 <> 1 Then
                        Me.PaintItemL2(mainbmpbig, p23, 4, SWR_lvl3_xpos, SWR_lvl3_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, SWRIGHT_L03, MainWindow_width + SWR_lvl3_xpos, SWR_lvl3_ypos)
                        Me.PaintItemL2(mainbmpbig, p23, 3, MW_lvl2_xpos + WALL_L02.Width, MW_lvl2_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L02_right, MainWindow_width + WALL_L02.Width + WALL_L02_left.Width - 2, MW_lvl2_ypos)
                    End If
                    If p11 <> 1 Then
                        Me.PaintItemL1(mainbmpbig, p11, 0, SWL_lvl2_xpos, SWL_lvl2_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, SWLEFT_L02, MainWindow_width + SWL_lvl2_xpos, SWL_lvl2_ypos)
                    End If
                    If p13 <> 1 Then
                        Me.PaintItemL1(mainbmpbig, p13, 4, SWR_lvl2_xpos, SWR_lvl2_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, SWRIGHT_L02, MainWindow_width + SWR_lvl2_xpos, SWR_lvl2_ypos)
                    End If
                    If p10 <> 1 Then
                        Me.PaintItemL1(mainbmpbig, p10, 5, SWL_lvl1_xpos, SWL_lvl1_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, SWLEFT_L01, MainWindow_width + SWL_lvl1_xpos, SWL_lvl1_ypos)
                    Else
                        If p11 <> 1 Then 'draw second lvl1 wall left
                            Me.PaintItemL1(mainbmpbig, p11, 1, MW_lvl1_xpos - WALL_L01.Width, MW_lvl1_ypos)
                            'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L01_left, MainWindow_width, MW_lvl1_ypos)
                        End If
                    End If
                    If p01 <> 1 Then
                        Me.PaintItemL1(mainbmpbig, p01, 6, SWR_lvl1_xpos, SWR_lvl1_ypos)
                        'mainbmpbig = Me.AddSprite(mainbmpbig, SWRIGHT_L01, MainWindow_width + SWR_lvl1_xpos, SWR_lvl1_ypos)
                    Else
                        If p13 <> 1 Then 'draw second lvl1 wall left
                            Me.PaintItemL1(mainbmpbig, p13, 3, MW_lvl1_xpos + WALL_L01.Width, MW_lvl1_ypos)
                            'mainbmpbig = Me.AddSprite(mainbmpbig, WALL_L01_right, MainWindow_width + WALL_L01.Width + WALL_L01_left.Width - 2, MW_lvl1_ypos)
                        End If
                    End If

                End If
            End If
        End If

        'mainbmp = Me.AddSprite(mainbmp, STAIRS01A, STAIRS_xpos, STAIRS_ypos)
        Me.CopyFromBigGameMap(mainbmp2, mainbmpbig)
        PictureBox1.Image = mainbmp2
        'PictureBox1.Image = mainbmp

    End Sub

    Public Sub PaintTheItems(ByVal px As Integer, ByVal py As Integer, ByVal dir As String)
        Dim item As Integer
        Dim bmp As Bitmap

        bmp = PictureBox1.Image
        item = MyItems(px, py)
        Select Case item

            Case 61 'Keyhole
                bmp = Me.AddSpriteTransWhite(bmp, KEYHOLE01, KeyholePosx, KeyholePosy)

            Case 81 'Pushbutton
                bmp = Me.AddSpriteTransWhite(bmp, PUSH01, PushbuttonPosx, PushbuttonPosy)
            Case 0
                Return

            Case Else
                'bmp = Me.AddSpriteTransWhite(bmp, PUSH01, PushbuttonPosx, PushbuttonPosy)


        End Select



    End Sub


    Public Sub PaintWallsNorth()
        Dim posx, posy As Integer

        posx = CurrentPosX3
        posy = CurrentPosY3

        p00 = MyFieldsB(posx, posy)
        p10 = MyFieldsB(posx - 1, posy)
        P01 = MyFieldsB(posx + 1, posy)

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
        Me.PaintTheItems(CurrentPosX, CurrentPosY - 1, "North")


    End Sub


    Public Sub PaintWallsWest()

        Dim posx, posy As Integer
        Dim mainbmp As New Bitmap(355, 240)

        mainbmp = Bitmap.FromFile(GameFile + "\picts\EOB-BACKDROP-1.bmp")
        posx = CurrentPosX3
        posy = CurrentPosY3
        p10 = MyFieldsB(posx, posy + 1)
        p00 = MyFieldsB(posx, posy)
        P01 = MyFieldsB(posx, posy - 1)
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
        Me.PaintTheItems(CurrentPosX - 1, CurrentPosY, "West")

    End Sub


    Public Sub PaintWallsEast()
        Dim posx, posy As Integer
        Dim mainbmp As New Bitmap(355, 240)

        mainbmp = Bitmap.FromFile(GameFile + "\picts\EOB-BACKDROP-1.bmp")
        posx = CurrentPosX3
        posy = CurrentPosY3
        p00 = MyFieldsB(posx, posy)
        p10 = MyFieldsB(posx, posy - 1)
        P01 = MyFieldsB(posx, posy + 1)
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
        Me.PaintTheItems(CurrentPosX + 1, CurrentPosY, "East")

    End Sub

    Public Sub PaintWallsSouth()
        Dim posx, posy As Integer
        Dim mainbmp As New Bitmap(355, 240)

        mainbmp = Bitmap.FromFile(GameFile + "\picts\EOB-BACKDROP-1.bmp")
        posx = CurrentPosX3
        posy = CurrentPosY3
        p00 = MyFieldsB(posx, posy)
        p10 = MyFieldsB(posx + 1, posy)
        P01 = MyFieldsB(posx - 1, posy)
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
        Me.PaintTheItems(CurrentPosX, CurrentPosY + 1, "South")

    End Sub

    Public Sub FillMapsDB(ByVal mapnr As Integer)
        Dim dsmaps As New DataSet
        Dim mysql As String
        Dim mystring As String
        Dim da As DbDataAdapter
        Dim myconnection1 As DbConnection
        Dim mapstr, curitem As String
        Dim ind As Integer

        mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
        myconnection1 = New OleDbConnection(mystring)

        da = New OleDbDataAdapter
        mysql = "select * from Maps"
        da.SelectCommand = New OleDbCommand(mysql, myconnection1)

        myconnection1.Open()

        da.Fill(dsmaps, "Maps")

        For x = 0 To 37
            For y = 0 To 37
                MyFieldsB(x, y) = 0
            Next
        Next

        ind = 0
        Select Case mapnr
            Case 1
                mapstr = dsmaps.Tables(0).Rows(1).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters

                Next

            Case 2
                mapstr = dsmaps.Tables(0).Rows(2).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

            Case 3
                mapstr = dsmaps.Tables(0).Rows(3).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

            Case 4
                mapstr = dsmaps.Tables(0).Rows(4).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

            Case 5
                mapstr = dsmaps.Tables(0).Rows(5).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

            Case 6
                mapstr = dsmaps.Tables(0).Rows(6).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

            Case 7
                mapstr = dsmaps.Tables(0).Rows(7).Item(5)
                ind = 0
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters

                Next

            Case 8
                mapstr = dsmaps.Tables(0).Rows(8).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

            Case 9
                mapstr = dsmaps.Tables(0).Rows(9).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

            Case 10
                mapstr = dsmaps.Tables(0).Rows(10).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

            Case 11
                mapstr = dsmaps.Tables(0).Rows(11).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

            Case 12
                mapstr = dsmaps.Tables(0).Rows(12).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

            Case 13
                mapstr = dsmaps.Tables(0).Rows(13).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

            Case 14
                mapstr = dsmaps.Tables(0).Rows(14).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

            Case 15
                mapstr = dsmaps.Tables(0).Rows(15).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

            Case 16
                mapstr = dsmaps.Tables(0).Rows(16).Item(5)
                For y = 0 To 31
                    For x = 0 To 31
                        curitem = mapstr.Substring(ind, 1)
                        MyFields(x, y) = CInt(curitem)
                        ind = ind + 2 'skip the , character
                        'MyItems(y, x) = MyItems1(x, y)
                    Next
                    'ind = ind + 2 'skip the vbcrlf characters
                Next

        End Select


        For x = 0 To 31
            For y = 0 To 31
                MyFieldsB(x + 3, y + 3) = MyFields(x, y)
            Next
        Next


    End Sub

    Public Sub FillMaps(ByVal mapnr As Integer)

        For x = 0 To 37
            For y = 0 To 37
                MyFieldsB(x, y) = 0
            Next
        Next

        Select Case mapnr
            Case 1
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields1(x, y)
                        MyItems(y, x) = MyItems1(x, y)
                    Next
                Next

            Case 2
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields2(x, y)
                        MyItems(y, x) = MyItems2(x, y)
                    Next
                Next
            Case 3
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields3(x, y)
                        MyItems(y, x) = MyItems3(x, y)
                    Next
                Next
            Case 4
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields4(x, y)
                        MyItems(y, x) = MyItems4(x, y)
                    Next
                Next

            Case 5
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields5(x, y)
                        MyItems(y, x) = MyItems5(x, y)
                    Next
                Next
            Case 6
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields6(x, y)
                        MyItems(y, x) = MyItems6(x, y)
                    Next
                Next
            Case 7
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields7(x, y)
                        MyItems(y, x) = MyItems7(x, y)
                    Next
                Next
            Case 8
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields8(x, y)
                        MyItems(y, x) = MyItems8(x, y)
                    Next
                Next
            Case 9
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields9(x, y)
                        MyItems(y, x) = MyItems9(x, y)
                    Next
                Next
            Case 10
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields10(x, y)
                        MyItems(y, x) = MyItems10(x, y)
                    Next
                Next
            Case 11
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields11(x, y)
                        MyItems(y, x) = MyItems11(x, y)
                    Next
                Next
            Case 12
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields12(x, y)
                        MyItems(y, x) = MyItems12(x, y)
                    Next
                Next
            Case 13
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields13(x, y)
                        MyItems(y, x) = MyItems13(x, y)
                    Next
                Next
            Case 14
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields14(x, y)
                        MyItems(y, x) = MyItems14(x, y)
                    Next
                Next
            Case 15
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields15(x, y)
                        MyItems(y, x) = MyItems15(x, y)
                    Next
                Next
            Case 16
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields16(x, y)
                        MyItems(y, x) = MyItems16(x, y)
                    Next
                Next
            Case 17
                For x = 0 To 31
                    For y = 0 To 31
                        MyFields(y, x) = MyFields17(x, y)
                        MyItems(y, x) = MyItems17(x, y)
                    Next
                Next
                'Case 18
                '    For x = 0 To 31
                '        For y = 0 To 31
                '            MyFields(y, x) = MyFields18(x, y)
                '            MyItems(y, x) = MyItems18(x, y)
                '        Next
                '    Next
                'Case 19
                '    For x = 0 To 31
                '        For y = 0 To 31
                '            MyFields(y, x) = MyFields19(x, y)
                '            MyItems(y, x) = MyItems19(x, y)

                '        Next
                '    Next
                'Case 20
                '    For x = 0 To 31
                '        For y = 0 To 31
                '            MyFields(y, x) = MyFields20(x, y)
                '            MyItems(y, x) = MyItems20(x, y)
                '        Next
                '    Next

        End Select


        For x = 0 To 31
            For y = 0 To 31
                MyFieldsB(x + 3, y + 3) = MyFields(x, y)
            Next
        Next


    End Sub

    Private Sub InitializeBitmaps()

        'Load the Bitmaps

        'GameField = Bitmap.FromFile(GameFile + "\picts\EOB-BACKDROP-1.bmp")
        NorthBitmap = Bitmap.FromFile(GameFile + "\picts\North.png")
        EastBitmap = Bitmap.FromFile(GameFile + "\picts\East.png")
        SouthBitmap = Bitmap.FromFile(GameFile + "\picts\South.png")
        WestBitmap = Bitmap.FromFile(GameFile + "\picts\West.png")
        SWLEFT_L01 = Bitmap.FromFile(GameFile + "\walls\SW-LEFT-L01.png")
        SWLEFT_L02 = Bitmap.FromFile(GameFile + "\walls\SW-LEFT-L02.png")
        SWLEFT_L03 = Bitmap.FromFile(GameFile + "\walls\SW-LEFT-L03.png")
        SWLEFT_L04 = Bitmap.FromFile(GameFile + "\walls\SW-LEFT-L04.png")
        SWRIGHT_L01 = Bitmap.FromFile(GameFile + "\walls\SW-RIGHT-L01.png")
        SWRIGHT_L02 = Bitmap.FromFile(GameFile + "\walls\SW-RIGHT-L02.png")
        SWRIGHT_L03 = Bitmap.FromFile(GameFile + "\walls\SW-RIGHT-L03.png")
        SWRIGHT_L04 = Bitmap.FromFile(GameFile + "\walls\SW-RIGHT-L04.png")
        WALL_L01 = Bitmap.FromFile(GameFile + "\walls\WALL-L01.bmp")
        WALL_L02 = Bitmap.FromFile(GameFile + "\walls\WALL-L02.bmp")
        WALL_L03 = Bitmap.FromFile(GameFile + "\walls\WALL-L03.bmp")
        STAIRS01A = Bitmap.FromFile(GameFile + "\stairs\stairsdown.bmp")
        STAIRS02A = Bitmap.FromFile(GameFile + "\stairs\stairsdownlvl2.bmp")
        STAIRS03A = Bitmap.FromFile(GameFile + "\stairs\stairsdownlvl3.bmp")
        STAIRS01B = Bitmap.FromFile(GameFile + "\stairs\stairsup.bmp")
        STAIRS02B = Bitmap.FromFile(GameFile + "\stairs\stairsuplvl2.bmp")
        STAIRS03B = Bitmap.FromFile(GameFile + "\stairs\stairsuplvl3.bmp")

        KEYHOLE01 = Bitmap.FromFile(GameFile + "\keys\keyhole01.bmp")
        TELEPORT = Bitmap.FromFile(GameFile + "\portal\T001.gif")
        PUSH01 = Bitmap.FromFile(GameFile + "\push\PUSH01-L01.bmp")
        SWORD = Bitmap.FromFile(GameFile + "\weapons\sword.bmp")
        SHIELD = Bitmap.FromFile(GameFile + "\weapons\shield.bmp")
        MISS = Bitmap.FromFile(GameFile + "\health\MISS.bmp")
        HIT = Bitmap.FromFile(GameFile + "\health\HIT02.bmp")
        WOUND = Bitmap.FromFile(GameFile + "\health\WOUND02.bmp")

        EMPTY01 = Bitmap.FromFile(GameFile + "\misc\bg-empty.bmp")

        'Intialize the positions of Bitmaps
        KeyholePosx = WALL_L01.Width / 1.6
        KeyholePosy = WALL_L01.Height / 2.2
        PushbuttonPosx = WALL_L01.Width / 1.6
        PushbuttonPosy = WALL_L01.Height / 2

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim mainbmp As Bitmap

        'level 1 sidewalls
        mainbmp = PictureBox1.Image
        Dim bmp1 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-LEFT-L01.png")
        mainbmp = Me.AddSprite(mainbmp, bmp1, SWL_lvl1_xpos, SWL_lvl1_ypos)
        Dim bmp2 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-RIGHT-L01.png")
        mainbmp = Me.AddSprite(mainbmp, bmp2, SWR_lvl1_xpos, SWR_lvl1_ypos)

        'level 2 sidewalls
        Dim bmp3 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-LEFT-L02.png")
        mainbmp = Me.AddSprite(mainbmp, bmp3, SWL_lvl2_xpos, SWL_lvl2_ypos)
        Dim bmp4 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-RIGHT-L02.png")
        mainbmp = Me.AddSprite(mainbmp, bmp4, SWR_lvl2_xpos, SWR_lvl2_ypos)

        'level 3 sidewalls
        Dim bmp5 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-LEFT-L03.png")
        mainbmp = Me.AddSprite(mainbmp, bmp5, SWL_lvl3_xpos, SWL_lvl3_ypos)
        Dim bmp6 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-RIGHT-L03.png")
        mainbmp = Me.AddSprite(mainbmp, bmp6, SWR_lvl3_xpos, SWR_lvl3_ypos)

        'level 4 sidewalls
        Dim bmp7 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-LEFT-L04.png")
        mainbmp = Me.AddSprite(mainbmp, bmp7, SWL_lvl4_xpos, SWL_lvl4_ypos)
        Dim bmp8 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-RIGHT-L04.png")
        mainbmp = Me.AddSprite(mainbmp, bmp8, SWR_lvl4_xpos, SWR_lvl4_ypos)

        PictureBox1.Image = mainbmp

    End Sub

    Public Function AddSpriteTransWhite(ByRef mypict As Bitmap, ByVal sprite As Bitmap, ByVal xpos As Integer, ByVal ypos As Integer) As Bitmap

        Dim MyColor As Color
        Dim lx, ly, i As Integer
        Dim rect As RectangleF

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

    End Function


    Public Function AddSpriteTransBlack(ByRef mypict As Bitmap, ByVal sprite As Bitmap, ByVal xpos As Integer, ByVal ypos As Integer) As Bitmap

        Dim MyColor As Color
        Dim lx, ly, i As Integer
        Dim rect As RectangleF

        'pixfmt = sprite.GetPixelFormatSize()
        rect = sprite.GetBounds(GraphicsUnit.Pixel)
        lx = rect.Width
        ly = rect.Height
        'sprite.MakeTransparent()
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

    End Function

    Public Function AddSprite(ByRef mypict As Bitmap, ByVal sprite As Bitmap, ByVal xpos As Integer, ByVal ypos As Integer) As Bitmap
        Dim MyColor As Color
        Dim lx, ly As Integer
        Dim rect As RectangleF

        'pixfmt = sprite.GetPixelFormatSize()
        rect = sprite.GetBounds(GraphicsUnit.Pixel)
        lx = rect.Width
        ly = rect.Height

        For x = 0 To lx - 1
            For y = 0 To ly - 1
                MyColor = sprite.GetPixel(x, y)
                mypict.SetPixel(xpos + x, ypos + y, MyColor)
            Next
        Next

        Return mypict

    End Function

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim mainbmp As Bitmap


        'level 1 sidewalls
        mainbmp = PictureBox1.Image

        'level 1 mainwall
        Dim bmp3 As Bitmap = Bitmap.FromFile(GameFile + "\picts\WALL-L01.bmp")
        mainbmp = Me.AddSprite(mainbmp, bmp3, MW_lvl1_xpos, MW_lvl1_ypos)

        Dim bmp1 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-LEFT-L01.png")
        mainbmp = Me.AddSprite(mainbmp, bmp1, SWL_lvl1_xpos, SWL_lvl1_ypos)
        Dim bmp2 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-RIGHT-L01.png")
        mainbmp = Me.AddSprite(mainbmp, bmp2, SWR_lvl1_xpos, SWR_lvl1_ypos)
        PictureBox1.Image = mainbmp

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim mainbmp As Bitmap

        mainbmp = PictureBox1.Image


        'level 2 mainwall
        Dim bmp5 As Bitmap = Bitmap.FromFile(GameFile + "\picts\WALL-L02.bmp")
        mainbmp = Me.AddSprite(mainbmp, bmp5, MW_lvl2_xpos, MW_lvl2_ypos)

        'level 1 sidewalls
        Dim bmp1 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-LEFT-L01.png")
        mainbmp = Me.AddSprite(mainbmp, bmp1, SWL_lvl1_xpos, SWL_lvl1_ypos)
        Dim bmp2 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-RIGHT-L01.png")
        mainbmp = Me.AddSprite(mainbmp, bmp2, SWR_lvl1_xpos, SWR_lvl1_ypos)

        'level 2 sidewalls
        Dim bmp3 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-LEFT-L02.png")
        mainbmp = Me.AddSprite(mainbmp, bmp3, SWL_lvl2_xpos, SWL_lvl2_ypos)
        Dim bmp4 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-RIGHT-L02.png")
        mainbmp = Me.AddSprite(mainbmp, bmp4, SWR_lvl2_xpos, SWR_lvl2_ypos)

        PictureBox1.Image = mainbmp


    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim mainbmp As Bitmap

        mainbmp = PictureBox1.Image

        'level 3 mainwall
        Dim bmp7 As Bitmap = Bitmap.FromFile(GameFile + "\picts\WALL-L03.bmp")
        mainbmp = Me.AddSprite(mainbmp, bmp7, MW_lvl3_xpos, MW_lvl3_ypos)

        'level 1 sidewalls
        'mainbmp = PictureBox1.Image
        'Dim bmp1 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-LEFT-L01.png")
        'mainbmp = Me.AddSprite(mainbmp, bmp1, SWL_lvl1_xpos, SWL_lvl1_ypos)
        'Dim bmp2 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-RIGHT-L01.png")
        'mainbmp = Me.AddSprite(mainbmp, bmp2, SWR_lvl1_xpos, SWR_lvl1_ypos)

        ''level 2 sidewalls
        'Dim bmp3 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-LEFT-L02.png")
        'mainbmp = Me.AddSprite(mainbmp, bmp3, SWL_lvl2_xpos, SWL_lvl2_ypos)
        'Dim bmp4 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-RIGHT-L02.png")
        'mainbmp = Me.AddSprite(mainbmp, bmp4, SWR_lvl2_xpos, SWR_lvl2_ypos)

        ''level 3 sidewalls
        'Dim bmp5 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-LEFT-L03.png")
        'mainbmp = Me.AddSprite(mainbmp, bmp5, SWL_lvl3_xpos, SWL_lvl3_ypos)
        'Dim bmp6 As Bitmap = Bitmap.FromFile(GameFile + "\picts\SW-RIGHT-L03.png")
        'mainbmp = Me.AddSprite(mainbmp, bmp6, SWR_lvl3_xpos, SWR_lvl3_ypos)


        PictureBox1.Image = mainbmp

    End Sub


    Public Sub GetPortalPositions(ByVal mylvl As Integer, ByVal posx As Integer, ByVal posy As Integer, ByRef newlvl As Integer, ByRef newx As Integer, ByRef newy As Integer)

        Dim mysql As String
        Dim mystring As String
        Dim myconnection1 As DbConnection
        Dim mycommand As DbCommand

        mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
        myconnection1 = New OleDbConnection(mystring)

        mysql = "select target from Portals where gameid = 1 and levelid = " + mylvl.ToString() + " and posx = " + posx.ToString() + " and posy = " + posy.ToString()

        mycommand = New OleDbCommand
        mycommand.CommandText = mysql
        mycommand.Connection = myconnection1
        myconnection1.Open()
        newlvl = mycommand.ExecuteScalar()

        mysql = "select tposx from Portals where gameid = 1 and levelid = " + mylvl.ToString() + " and posx = " + posx.ToString() + " and posy = " + posy.ToString()
        mycommand.CommandText = mysql
        newx = mycommand.ExecuteScalar()

        mysql = "select tposy from Portals where gameid = 1 and levelid = " + mylvl.ToString() + " and posx = " + posx.ToString() + " and posy = " + posy.ToString()
        mycommand.CommandText = mysql
        newy = mycommand.ExecuteScalar()


    End Sub


    Public Sub GetNewPositions(ByVal mylvl As Integer, ByVal posx As Integer, ByVal posy As Integer, ByRef newlvl As Integer, ByRef newx As Integer, ByRef newy As Integer)
        Dim mysql As String
        Dim mystring As String
        Dim myconnection1 As DbConnection
        Dim mycommand As DbCommand

        mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
        myconnection1 = New OleDbConnection(mystring)

        mysql = "select target from Treps where gameid = 1 and levelid = " + mylvl.ToString() + " and posx = " + posx.ToString() + " and posy = " + posy.ToString()

        mycommand = New OleDbCommand
        mycommand.CommandText = mysql
        mycommand.Connection = myconnection1
        myconnection1.Open()
        newlvl = mycommand.ExecuteScalar()

        mysql = "select tposx from Treps where gameid = 1 and levelid = " + mylvl.ToString() + " and posx = " + posx.ToString() + " and posy = " + posy.ToString()
        mycommand.CommandText = mysql
        newx = mycommand.ExecuteScalar()

        mysql = "select tposy from Treps where gameid = 1 and levelid = " + mylvl.ToString() + " and posx = " + posx.ToString() + " and posy = " + posy.ToString()
        mycommand.CommandText = mysql
        newy = mycommand.ExecuteScalar()



    End Sub


    Public Sub ShowCounters()

        Tlvl.Text = CurrentLevel.ToString()
        Tposx.Text = CurrentPosX.ToString()
        Tposy.Text = CurrentPosY.ToString()
        Tposx3.Text = CurrentPosX3.ToString()
        Tposy3.Text = CurrentPosY3.ToString()

        Select Case CurrentDirection

            Case 0
                Torientation.Text = "North"
            Case 1
                Torientation.Text = "East"
            Case 2
                Torientation.Text = "South"
            Case 3
                Torientation.Text = "West"
        End Select

        Select Case CurrentView

            Case 0
                Tview.Text = "North"
            Case 1
                Tview.Text = "East"
            Case 2
                Tview.Text = "South"
            Case 3
                Tview.Text = "West"
        End Select

    End Sub


    Private Sub PBchars01_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Public Sub LoadMonster()

        MONSTERPIC1 = Bitmap.FromFile(CurDir() + "\monsters\" + MONSTERPICNAME1)
        MONSTERPIC2 = Bitmap.FromFile(CurDir() + "\monsters\" + MONSTERPICNAME2)
        MONSTERPIC3 = Bitmap.FromFile(CurDir() + "\monsters\" + MONSTERPICNAME3)

    End Sub
    Private Sub Button15_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button15.Click

        FightGoingOn = True

        Me.LoadMonster()
        Dim bmp1 As Bitmap = MONSTERPIC1

        p_monster.Image = bmp1
        p_monster.BackColor = Color.Transparent


        p_monster.BackgroundImage = PictureBox1.Image
        p_monster.Visible = True
        p_health.Width = p_health.Width * 0.9
        p_health2.Visible = True
        p_health.Visible = True
        p_health.BringToFront()



    End Sub


    Private Sub PictureBox1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles PictureBox1.MouseDown

        MsgBox(e.X)
        MsgBox(e.Y)
    End Sub

    Private Sub W01_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        If FightGoingOn = True Then
            'check probability of hit (random)
            'when hit: random hitpoints
            'show hitpoints on monster

            'lower hitpoints from monster health
            'update health on bitmap

            W01.Image = MISS


        Else
            Return
        End If
    End Sub

    Private Sub Button13_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button13.Click

        FightGoingOn = False
        p_monster.Visible = False
        p_health.Visible = False
        p_health2.Visible = False


    End Sub


    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim item As String

        item = ComboBox1.Text

        Select Case item

            Case "gargoyle"
                MONSTERPICNAME1 = "gargoyle.gif"
            Case "ant"
                MONSTERPICNAME1 = "ant.gif"
            Case "aservant"
                MONSTERPICNAME1 = "aservant.gif"
            Case "basilisk"
                MONSTERPICNAME1 = "basilisk.gif"
            Case "beholder"
                MONSTERPICNAME1 = "beholder.gif"
            Case "bullette"
                MONSTERPICNAME1 = "bullette.gif"
            Case "cleric"
                MONSTERPICNAME1 = "cleric.gif"
            Case "cube"
                MONSTERPICNAME1 = "cube.gif"
            Case "dragon"
                MONSTERPICNAME1 = "dragon.gif"
            Case "giant"
                MONSTERPICNAME1 = "giant.gif"
            Case "guard"
                MONSTERPICNAME1 = "guard.gif"
            Case "guardian"
                MONSTERPICNAME1 = "guardian.gif"
            Case "hellhound"
                MONSTERPICNAME1 = "hellhound.gif"
            Case "mage"
                MONSTERPICNAME1 = "mage.gif"
            Case "mantis"
                MONSTERPICNAME1 = "mantis.gif"
            Case "medusa"
                MONSTERPICNAME1 = "medusa.gif"
            Case "mindflayer"
                MONSTERPICNAME1 = "mindflayer.gif"
            Case "salamander"
                MONSTERPICNAME1 = "salamander.gif"
            Case "skeletwarrior"
                MONSTERPICNAME1 = "skeletwarrior.gif"
            Case "snake"
                MONSTERPICNAME1 = "snake.gif"
            Case "spider"
                MONSTERPICNAME1 = "spider.gif"
            Case "wasp"
                MONSTERPICNAME1 = "wasp.gif"
            Case "willowwisp"
                MONSTERPICNAME1 = "willowwisp.gif"

        End Select
    End Sub

End Class

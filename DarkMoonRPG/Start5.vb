Imports System.Drawing.Imaging
Imports System.Drawing

Public Class Start5
    'Identifiers for wall sprites manipulation

    'Dim BMP As Bitmap
    'Dim Rotate As Integer = 0
    'Dim RStep As Integer = -1
    'Dim PWidth As Integer = 0
    'Dim Offset As Integer = 0


    Public MainWindow_width As Integer = 355
    Public MainWindow_height As Integer = 240

    'Main walls positions
    Public MW_lvl1_xpos As Integer = 0
    Public MW_lvl1_ypos As Integer = 2
    Public MW_lvl1_width As Integer = 262
    Public MW_lvl1_height As Integer = 240

    Public MW_lvl2_xpos As Integer = 0
    Public MW_lvl2_ypos As Integer = 0
    Public MW_lvl2_width As Integer = 355
    Public MW_lvl2_height As Integer = 240

    Public MW_lvl3_xpos As Integer = 0
    Public MW_lvl3_ypos As Integer = 0
    Public MW_lvl3_width As Integer = 352
    Public MW_lvl3_height As Integer = 240



    'Sidewalls level 1
    Public SWL_lvl1_xpos As Integer = 0
    Public SWL_lvl1_ypos As Integer = 0
    Public SWL_lvl1_width As Integer = 52
    Public SWL_lvl1_height As Integer = 239
    Public SWR_lvl1_xpos As Integer = 303
    Public SWR_lvl1_ypos As Integer = 0
    Public SWR_lvl1_width As Integer = 52
    Public SWR_lvl1_height As Integer = 239

    'Sidewalls level 2
    Public SWL_lvl2_xpos As Integer = 51
    Public SWL_lvl2_ypos As Integer = 13
    Public SWL_lvl2_width As Integer = 47
    Public SWL_lvl2_height As Integer = 195
    Public SWR_lvl2_xpos As Integer = 259
    Public SWR_lvl2_ypos As Integer = 13
    Public SWR_lvl2_width As Integer = 47
    Public SWR_lvl2_height As Integer = 195

    'Sidewalls level 3
    Public SWL_lvl3_xpos As Integer = 97
    Public SWL_lvl3_ypos As Integer = 38
    Public SWL_lvl3_width As Integer = 30
    Public SWL_lvl3_height As Integer = 118
    Public SWR_lvl3_xpos As Integer = 229
    Public SWR_lvl3_ypos As Integer = 38

    'Sidewalls level 4
    Public SWL_lvl4_xpos As Integer = 127
    Public SWL_lvl4_ypos As Integer = 50
    Public SWL_lvl4_width As Integer = 6
    Public SWL_lvl4_height As Integer = 84
    Public SWR_lvl4_xpos As Integer = 223
    Public SWR_lvl4_ypos As Integer = 50


    Public GameFolder As String = ""
    Public GameFile As String = ""
    Public ds As New DataSet

    Private Sub Start5_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        GameFile = CurDir()
        ds.ReadXml(GameFile + "\xml\eob2.xml")
        GameFolder = ds.Tables(0).Rows(0).Item(0)

        Dim bmp As Bitmap = Bitmap.FromFile(GameFile + "\picts\EOB-BACKDROP-1.bmp")
        PictureBox1.Image = bmp

        PictureBox1.Width = MainWindow_width
        PictureBox1.Height = MainWindow_height


    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim mainbmp As Bitmap
        Dim MyColor As Color

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



    Public Function AddSprite2(ByVal mypict As Bitmap, ByVal sprite As Bitmap, ByVal xpos As Integer, ByVal ypos As Integer) As Bitmap
        Dim MyColor As Color
        Dim lx, ly As Integer
        Dim rect As RectangleF
        Dim newpict As Bitmap

        'pixfmt = sprite.GetPixelFormatSize()
        rect = sprite.GetBounds(GraphicsUnit.Pixel)
        lx = rect.Width
        ly = rect.Height

        newpict = mypict
        For x = 0 To lx - 1
            For y = 0 To ly - 1
                MyColor = sprite.GetPixel(x, y)
                newpict.SetPixel(xpos + x, ypos + y, MyColor)
            Next
        Next

        Return newpict


    End Function


    Public Function AddSprite(ByRef mypict As Bitmap, ByVal sprite As Bitmap, ByVal xpos As Integer, ByVal ypos As Integer) As Bitmap
        Dim MyColor As Color
        Dim lx, ly As Integer
        Dim pixfmt As System.Drawing.Imaging.PixelFormat
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
        Dim MyColor As Color


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
        Dim MyColor As Color


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
        Dim MyColor As color

        mainbmp = PictureBox1.Image

        'level 3 mainwall
        Dim bmp7 As Bitmap = Bitmap.FromFile(GameFile + "\picts\WALL-L03.bmp")
        mainbmp = Me.AddSprite(mainbmp, bmp7, MW_lvl3_xpos, MW_lvl3_ypos)

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


        PictureBox1.Image = mainbmp


    End Sub

    Public Function RotateImg(ByVal bmpimage As Bitmap, ByVal angle As Single) As Bitmap
        Dim w As Integer = bmpimage.Width
        Dim h As Integer = bmpimage.Height
        Dim pf As System.Drawing.Imaging.PixelFormat = Nothing

        pf = bmpimage.PixelFormat
        Dim tempImg As New Bitmap(w, h, pf)
        Dim g As Graphics = Graphics.FromImage(tempImg)
        g.DrawImageUnscaled(bmpimage, 1, 1)
        g.Dispose()
        Dim path As New System.Drawing.Drawing2D.GraphicsPath()

        path.AddRectangle(New RectangleF(0.0F, 0.0F, w, h))
        Dim mtrx As New System.Drawing.Drawing2D.Matrix()

        mtrx.Rotate(angle)
        Dim rct As RectangleF = path.GetBounds(mtrx)
        Dim newImg As New Bitmap(Convert.ToInt32(rct.Width), Convert.ToInt32(rct.Height), pf)
        g = Graphics.FromImage(newImg)
        g.TranslateTransform(-rct.X, -rct.Y)
        g.RotateTransform(angle)
        'g.InterpolationMode = InterpolationMode.HighQualityBilinear

        g.DrawImageUnscaled(tempImg, 0, 0)
        g.Dispose()
        tempImg.Dispose()
        Return newImg

    End Function

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        Dim mainbmp As Bitmap
        Dim MyColor As Color


        mainbmp = PictureBox1.Image
        mainbmp = RotateImg(mainbmp, CInt(TextBox1.Text))
        PictureBox1.Image = mainbmp



    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click


        'BMP = Image.FromFile(GameFile + "\picts\WALL-L03.bmp")
        'Timer1.Enabled = True


    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick

        'Rotate += RStep
        'If Rotate = 0 Then
        '    BMP.RotateFlip(RotateFlipType.RotateNoneFlipY)
        'End If
        'If Rotate = 100 Then
        '    RStep = -1
        'End If
        'If Rotate = -100 Then
        '    RStep = 1
        'End If
        'PWidth = Rotate * BMP.Width / 100
        'Offset = BMP.Width - PWidth / 2
        'Me.Invalidate()

    End Sub

    Private Sub Start5_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

        'If Not BMP Is Nothing Then
        '    e.Graphics.DrawImage(BMP, New Rectangle(Offset, 40, PWidth, BMP.Height))
        'End If

    End Sub
End Class
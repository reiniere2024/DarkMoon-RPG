Imports System.Runtime.InteropServices

Module Cursors


    Public Class create

#Region "   CreateIconIndirect"

        Private Structure IconInfo
            Public fIcon As Boolean
            Public xHotspot As Int32
            Public yHotspot As Int32
            Public hbmMask As IntPtr
            Public hbmColor As IntPtr
        End Structure

        <DllImport("user32.dll", EntryPoint:="CreateIconIndirect")> _
        Private Shared Function CreateIconIndirect(ByVal iconInfo As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)> _
        Public Shared Function DestroyIcon(ByVal handle As IntPtr) As Boolean
        End Function

        <DllImport("gdi32.dll")> _
        Public Shared Function DeleteObject(ByVal hObject As IntPtr) As Boolean
        End Function

        ''' <summary>
        ''' CreateCursor
        ''' </summary>
        ''' <param name="bmp"></param>
        ''' <returns>custom Cursor</returns>
        ''' <remarks>creates a custom cursor from a bitmap</remarks>
        Public Shared Function CreateCursor(ByVal bmp As Bitmap, ByVal xHotspot As Integer, ByVal yHotspot As Integer) As Cursor
            'Setup the Cursors IconInfo
            Dim tmp As New IconInfo
            tmp.xHotspot = xHotspot
            tmp.yHotspot = yHotspot
            tmp.fIcon = False
            tmp.hbmMask = bmp.GetHbitmap()
            tmp.hbmColor = bmp.GetHbitmap()

            'Create the Pointer for the Cursor Icon
            Dim pnt As IntPtr = Marshal.AllocHGlobal(Marshal.SizeOf(tmp))
            Marshal.StructureToPtr(tmp, pnt, True)
            Dim curPtr As IntPtr = CreateIconIndirect(pnt)

            'Clean Up
            DestroyIcon(pnt)
            DeleteObject(tmp.hbmMask)
            DeleteObject(tmp.hbmColor)

            Return New Cursor(curPtr)
        End Function

#End Region

    End Class


End Module

Imports System.IO
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.Common

Module GameResources

    Public Class Trep
        Public Level As Integer
        Public xpos As Integer
        Public ypos As Integer
        Public TargetLevel As Integer
        Public trg_xpos As Integer
        Public trg_ypos As Integer

    End Class


    Public Class Character

        Public Charnr As Integer
        Public Picnr As Integer

        Public Name As String
        Public Gender As String
        Public Race As String
        Public RClass As String
        Public Strengh As Integer
        Public Intelligence As Integer
        Public Wisdom As Integer
        Public Dexterity As Integer
        Public Constitution As Integer
        Public Charisma As Integer
        Public Portrait As Bitmap
        Public Level As Integer
        Public Health As Integer
        Public Mana As Integer
        Public GameFile As String = ""
        Public MyWindow As Mainwindow2
        Public Hand1 As Integer
        Public Hand2 As Integer
        Public InventoryItems(14) As Integer

        Public Sub FillInventory()

            Select Case Charnr

                Case 1
                    InventoryItems(0) = ItemPotion
                    InventoryItems(1) = ItemSword
                    InventoryItems(2) = ItemAxe
                    InventoryItems(3) = ItemMace
                    InventoryItems(4) = ItemShield

                Case 2
                    InventoryItems(0) = ItemPotion
                    InventoryItems(1) = ItemSword
                    InventoryItems(2) = ItemFlail
                    InventoryItems(3) = ItemShield

                Case 3
                    InventoryItems(0) = ItemPotion
                    InventoryItems(1) = SpellBurningHands
                    InventoryItems(2) = SpellFireball
                    InventoryItems(3) = SpellDisintegration

                Case 4
                    InventoryItems(0) = ItemPotion
                    InventoryItems(1) = SpellBurningHands
                    InventoryItems(2) = SpellFireball
                    InventoryItems(3) = SpellFleshtoStone

            End Select

        End Sub

        Public Sub StoreInventory(ByVal cnr As Integer)

            Dim mysql, mydoor As String
            Dim mystring As String
            Dim myconnection1 As DbConnection
            Dim mycommand As DbCommand

            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)
            mycommand = New OleDbCommand
            mycommand.Connection = myconnection1
            myconnection1.Open()

            'select hands
            mysql = "Update Inventory set hand1 = " + Hand1.ToString() + " where invnr = " + cnr.ToString()
            mycommand.CommandText = mysql
            mycommand.ExecuteNonQuery()

            'select hands
            mysql = "Update Inventory set hand2 = " + Hand2.ToString() + " where invnr = " + cnr.ToString()
            mycommand.CommandText = mysql
            mycommand.ExecuteNonQuery()

            'update inventory
            For i = 1 To 14
                mysql = "Update Inventory set inv" + i.ToString() + " = " + InventoryItems(i - 1).ToString() + " where invnr = " + cnr.ToString()
                mycommand.ExecuteNonQuery()
            Next
            myconnection1.Close()


        End Sub


        Public Sub LoadInventory(ByVal cnr As Integer)
            Dim mysql, mydoor As String
            Dim mystring As String
            Dim myconnection1 As DbConnection
            Dim mycommand As DbCommand

            mystring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='Database\RPGGame.mdb';Jet OLEDB:Database Password=Tapyxe01"
            myconnection1 = New OleDbConnection(mystring)
            mycommand = New OleDbCommand
            mycommand.Connection = myconnection1
            myconnection1.Open()
            'select hands
            mysql = "select hand1 from Inventory where invnr = " + cnr.ToString()
            mycommand.CommandText = mysql
            Hand1 = mycommand.ExecuteScalar()

            mysql = "select hand2 from Inventory where invnr = " + cnr.ToString()
            mycommand.CommandText = mysql
            Hand2 = mycommand.ExecuteScalar()
            'select inventory
            For i = 1 To 14
                mysql = "select inv" + i.ToString() + " from Inventory where invnr = " + cnr.ToString()
                mycommand.CommandText = mysql
                InventoryItems(i - 1) = mycommand.ExecuteScalar()
            Next

            myconnection1.Close()

        End Sub

        Sub New(ByRef myform As Mainwindow2, ByVal n As String, ByVal g As String, ByVal r As String, ByVal c As String, ByVal pnr As Integer, ByVal charnum As Integer)

            MyWindow = myform
            GameFile = CurDir()
            Charnr = charnum
            Name = n
            'PBNames(Charnr - 1).Text = n

            Gender = g
            Race = r
            RClass = c
            Health = MaxHealth
            Mana = MaxMana
            'Hand1 = ItemSword
            'Hand2 = ItemBook

            If Charnr < 3 Then
                '    Hand1 = ItemSword
                '    Hand2 = ItemShield
                Hand1 = ItemLeftHand
                Hand2 = ItemRightHand
            Else
                '    Hand1 = ItemSword
                '    Hand2 = ItemBook
                Hand1 = ItemLeftHand
                Hand2 = ItemRightHand
            End If

            Me.InitializeLevels()
            Picnr = pnr

            'Me.FillInventory()
            Me.LoadInventory(Charnr)

            Me.Showhand(1)
            Me.Showhand(2)
            Me.ShowHealth()




        End Sub

        Public Sub UpdateChar(ByVal newchar As Integer)

            Charnr = newchar

        End Sub


        Public Sub SetName(ByVal pname As String)

            Name = pname

        End Sub


        Public Function GetInventory(ByVal invnr As Integer) As Integer

            Return InventoryItems(invnr)

        End Function

        Public Sub UpdateInventory(ByVal invnr As Integer, ByVal item As Integer)

            InventoryItems(invnr) = item

        End Sub

        Public Function GetHand(ByVal handnr As Integer) As Integer
            Dim item As Integer

            If handnr = 1 Then
                item = Hand1
            ElseIf handnr = 2 Then
                item = Hand2
            End If
            Return item

        End Function

        Public Sub UpdateHealth(ByVal change As Integer)

            Health = Health - change
            Me.ShowHealth()

        End Sub

        Public Function GetHealth() As Integer

            Return Health

        End Function

        Public Sub UpdateHand(ByVal handnr As Integer, ByVal item As Integer)

            If handnr = 1 Then
                Hand1 = item
            ElseIf handnr = 2 Then
                Hand2 = item
            End If


        End Sub

        Public Sub ShowHealth()

            HEALTHPIC(Charnr - 1).Width = healthpic_width * (Health / 100)

        End Sub

        Public Sub Showhand(ByVal handnr As Integer)
            'Dim itempic As Bitmap
            Dim img As New Bitmap(30, 30)
            If handnr = 1 Then
                img = Getitem(Hand1)
            Else
                img = Getitem(Hand2)
            End If
            img.MakeTransparent(Color.Black)
            If handnr = 1 Then
                HAND1PIC(Charnr - 1).Image = img
            Else
                HAND2PIC(Charnr - 1).Image = img
            End If

        End Sub

        Public Sub ShowInventory(ByVal invnr As Integer)
            Dim img As New Bitmap(30, 30)

            img = Getitem(InventoryItems(invnr))
            img.MakeTransparent(Color.Black)
            PBInv(invnr).Image = img

        End Sub

        Public Sub ShowInventory()
            Dim img As New Bitmap(30, 30)

            For i = 0 To 13
                img = Getitem(InventoryItems(i))
                img.MakeTransparent(Color.Black)
                PBInv(i).Image = img
            Next

        End Sub


        Public Sub LoadWeapons()
            Dim sprite2 As New Bitmap(96, 51)


            Showhand(1)
            Showhand(2)


        End Sub


        Public Function LoadPortrait(ByVal pnr As Integer) As Bitmap
            Dim pictname As String
            Dim pict As Bitmap

            pictname = "\chars\character00" + pnr.ToString() + ".png"
            pict = Bitmap.FromFile(GameFile + pictname)

            Return pict

        End Function

        Public Function LoadDeath(ByVal pnr As Integer) As Bitmap
            Dim pictname As String
            Dim pict As Bitmap

            pictname = "\chars\character00" + pnr.ToString() + ".png"
            pict = Bitmap.FromFile(GameFile + pictname)

            Return pict

        End Function

        Public Sub UpdatePortrait(ByVal pict As Bitmap)

            Portrait = pict

        End Sub

        Public Function GetPortrait() As Bitmap

            Return Portrait 'PBPortrait(Charnr - 1).Image

        End Function

        Public Sub ShowPortrait()

            PBPortrait(Charnr - 1).Image = Portrait

        End Sub


        Public Sub InitializeLevels()

            Select Case RClass

                Case "FIGHTER"
                    Level = 1
                    Strengh = 18
                    Intelligence = 12
                    Wisdom = 12
                    Dexterity = 16
                    Constitution = 17
                    Charisma = 14

                Case "MAGE"
                    Level = 1
                    Strengh = 12
                    Intelligence = 18
                    Wisdom = 17
                    Dexterity = 14
                    Constitution = 14
                    Charisma = 14

                Case "CLERIC"
                    Level = 1
                    Strengh = 12
                    Intelligence = 16
                    Wisdom = 17
                    Dexterity = 12
                    Constitution = 13
                    Charisma = 14

                Case "PALADIN"
                    Level = 1
                    Strengh = 18
                    Intelligence = 14
                    Wisdom = 14
                    Dexterity = 16
                    Constitution = 17
                    Charisma = 15

                Case "RANGER"
                    Level = 1
                    Strengh = 17
                    Intelligence = 13
                    Wisdom = 13
                    Dexterity = 16
                    Constitution = 17
                    Charisma = 14

                Case "THIEF"
                    Level = 1
                    Strengh = 13
                    Intelligence = 12
                    Wisdom = 12
                    Dexterity = 15
                    Constitution = 16
                    Charisma = 14

                Case "FIGHTER/THIEF"
                    Level = 1
                    Strengh = 18
                    Intelligence = 12
                    Wisdom = 12
                    Dexterity = 16
                    Constitution = 17
                    Charisma = 14

                Case "FIGHTER/CLERIC"
                    Level = 1
                    Strengh = 18
                    Intelligence = 14
                    Wisdom = 14
                    Dexterity = 16
                    Constitution = 17
                    Charisma = 14

                Case "MAGE/CLERIC"
                    Level = 1
                    Strengh = 12
                    Intelligence = 18
                    Wisdom = 17
                    Dexterity = 14
                    Constitution = 14
                    Charisma = 14


            End Select
        End Sub

        Public Function Getitem(ByVal itemnr As Integer) As Bitmap
            Dim bmp As Bitmap
            Dim pictname, filename As String

            If itemnr = 0 Then
                bmp = EMPTYINV
                Return bmp
            End If

            pictname = MyWindow.GetItemPict(itemnr)
            filename = GameFile + "\items\" + pictname
            bmp = Bitmap.FromFile(filename)
            Return bmp

        End Function


    End Class

End Module

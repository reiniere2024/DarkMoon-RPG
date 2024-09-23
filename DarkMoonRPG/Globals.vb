Module Globals

    Public MyHits1(6) As Date
    Public MyHits2(6) As Date
    Public MyHitsTempo1 As Integer = 1500
    Public MyHitsTempo2 As Integer = 1500
    Public PosMonsterX As Integer = 0
    Public PosMonsterY As Integer = 0
    Public MaxSpells As Integer = 2
    Public MySpells() As Integer = {MaxSpells, MaxSpells, MaxSpells, MaxSpells, MaxSpells, MaxSpells}
    Public BigMap As Boolean = False
    Public InvVisible As Boolean = False
    Public GameCompleted As Boolean = False


    Public MonsterHitpoints As Integer = 25
    Public SwordHitpoints As Integer = 10
    Public BookHitpoints As Integer = 12
    Public MaxHealth As Integer = 100
    Public MaxMana As Integer = 100
    Public ProtectionShield As Decimal = 0.5

    Public ItemLeftHand As Integer = 101
    Public ItemRightHand As Integer = 102

    Public ItemSword As Integer = 103
    Public ItemAxe As Integer = 104
    Public ItemFlail As Integer = 105
    Public ItemMace As Integer = 106
    Public ItemBook As Integer = 110
    Public ItemShield As Integer = 112

    Public SpellBurningHands As Integer = 202
    Public SpellFireball As Integer = 203
    Public SpellDisintegration As Integer = 205
    Public SpellFleshtoStone As Integer = 206

    Public SpellUsed As Integer = 220

    Public ItemPotion As Integer = 902
    Public ItemUsedPotion As Integer = 906


    Public dsTreps, dsPortals, dsDoors, dsItemtypes, ds_server, dsGameField, dsMonsters, dsWallTypes As New DataSet
    Public dsItemsFloor, dsItemsWall As New DataSet
    Public dsUsers As New DataSet


    Public HM_length As Integer = 173
    Public FightGoingOn As Boolean = False
    Public DragDropStarted As Boolean = False
    Public DraggedItem As Integer = 0
    Public SourceDragControl As PictureBox
    Public TargetDragControl As PictureBox
    Public CurChar As Integer = 0 'Current clicked Character on Inventory-box
    Public TestSpells As Boolean = False
    Public CurrentMessage As String
    Public SpellsVisible As Boolean = False

    'Main Character Controls
    Public PBNames(6) As Label
    Public PBPortrait(6) As PictureBox
    Public HAND1PIC(6) As PictureBox
    Public HAND2PIC(6) As PictureBox
    Public HEALTHPIC(6) As PictureBox
    Public MANAPIC(6) As PictureBox

    Public HEALTHMONSTER As PictureBox
    Public CampButton As PictureBox


    'Character Inventory Controls
    Public PBInv(14) As PictureBox
    Public PBInvUser As PictureBox
    Public PBInvName As Label

    Public CurrentDirection As Integer = 0 ' 0=north,1=east,2=south,3-west
    Public CurrentView As Integer = 0 ' 0=north,1=east,2=south,3-west

    Public Backdrops() As Integer = {0, 4, 3, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    Public Walls() As Integer = {0, 4, 3, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    Public Doors() As Integer = {0, 3, 3, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    Public Stairs() As Integer = {0, 12, 3, 3, 3, 3, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1}

    Public GameFieldId As Integer = 1
    Public WalltypeId As Integer = 2 '2 1=
    Public BackdropId As Integer = 4 '1
    Public WallsId As Integer = 4


    Public StairsId As Integer = 12 '11 1=silver,2=temple ,3=catacomb ,5=red EOB1,6=silver EOB1,7=purple EOB1,
    '10=crimson,11= shaded silver
    Public DoorId As Integer = 3  '11 '1=silver,2=temple,3=catacomb,5=red EOB1,6=silver EOB1,7=purple EOB1,
    '8=sanctum EOB1,9=green EOB1,10=crimson,11=shaded silver

    Public CurrentLevel As Integer = 1 '2 '1
    Public CurrentPosX As Integer = 12 '12 '16 '16 '12 '11 '12
    Public CurrentPosY As Integer = 6 '6 '4 '12 '6 '15 '6
    Public LastPosX As Integer = 0
    Public lastPosY As Integer = 0

    Public CurrentPosX3 As Integer
    Public CurrentPosY3 As Integer
    Public MyFields(33, 33) As Integer       'Walls in Game-map
    Public MyFieldsB(39, 39) As Integer      'Walls in Game-map with Borders
    Public MyItems(33, 33) As Integer        'Items in Game-map
    Public MyItemsFloor(33, 33) As Integer   'Items on Floor
    Public MyItemsWall(33, 33) As Integer    'Items on Wall
    Public MyMonsters(33, 33) As Integer     'Monsters on Map
    Public MyMonsters1(33, 33) As Integer     'Monsters on Map
    Public MyMonsters2(33, 33) As Integer     'Monsters on Map
    Public MyMonsters3(33, 33) As Integer     'Monsters on Map
    Public MyMonsters4(33, 33) As Integer     'Monsters on Map
    Public MyMonsters5(33, 33) As Integer     'Monsters on Map
    Public MyMonsters6(33, 33) As Integer     'Monsters on Map
    Public MyMonsters7(33, 33) As Integer     'Monsters on Map
    Public MyMonsters8(33, 33) As Integer     'Monsters on Map
    Public MyMonsters9(33, 33) As Integer     'Monsters on Map
    Public MyMonsters10(33, 33) As Integer     'Monsters on Map
    Public MyMonsters11(33, 33) As Integer     'Monsters on Map
    Public MyMonsters12(33, 33) As Integer     'Monsters on Map
    Public MyMonsters13(33, 33) As Integer     'Monsters on Map
    Public MyMonsters14(33, 33) As Integer     'Monsters on Map
    Public MyMonsters15(33, 33) As Integer     'Monsters on Map
    Public MyMonsters16(33, 33) As Integer     'Monsters on Map


    'Game Positions for Movement
    Public p00, p10, p01, p11, p12, p13, p21, p22, p23, p30, p31, p32, p33, p34 As Integer

    Public FlipOperation As Boolean = False
    Public Flipsource As Integer = 0
    Public Fliptarget As Integer = 0

    Public GameFolder As String = ""
    Public GameFile As String = ""
    Public GameChars(4) As Character

    'Monster variables
    Public MONSTERPICNAME1 As String
    Public MONSTERPICNAME2 As String
    Public MONSTERPICNAME3 As String

    Public CurrentHealth As Integer = 100

    Private m_Alpha As Single = 0.0 ' Alpha on a 0-1 scale.
    Private m_DAlpha As Single = 0.5

    Public MonsterDebugging As Boolean = False



End Module

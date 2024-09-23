Module Positions

    Public MainWindow_width As Integer = 0 '355
    Public MainWindow_height As Integer = 0 '240
    Public MainWindow_xpos As Integer = 0 '142
    Public MainWindow_ypos As Integer = 0

    Public Inventory_xpos As Integer = 364
    Public Inventory_ypos As Integer = 0
    Public Inventory_width As Integer = 281
    Public Inventory_height As Integer = 318

    'Identifiers for items on ground and walls
    Public ItemFloor_xpos As Integer = 120
    Public ItemFloor_ypos As Integer = 180
    Public ItemFloor_width As Integer = 130
    Public ItemFloor_height As Integer = 35

    Public ItemWall_xpos As Integer = 160
    Public ItemWall_ypos As Integer = 75
    Public ItemWall_xpos2 As Integer = 160
    Public ItemWall_ypos2 As Integer = 75
    Public ItemWall_xpos3 As Integer = 160
    Public ItemWall_ypos3 As Integer = 75


    Public ItemWall_width As Integer = 50
    Public ItemWall_height As Integer = 50

    Public ItemPosx As Integer
    Public ItemPosy As Integer
    Public KeyholePosx As Integer '= WALL_L01.Width / 2.1
    Public KeyholePosy As Integer '= WALL_L01.Height / 2.5
    Public PushbuttonPosx As Integer
    Public PushbuttonPosy As Integer
    Public PushbuttonWidth As Integer
    Public PushbuttonHeight As Integer
    Public WeaponsPosx As Integer = 25
    Public WeaponsPosy As Integer = 5

    'Main walls dimensions
    Public MW_lvl1_width As Integer = 259
    Public MW_lvl1_height As Integer = 195
    Public MW_lvl2_width As Integer = 162
    Public MW_lvl2_height As Integer = 122
    Public MW_lvl3_width As Integer = 98
    Public MW_lvl3_height As Integer = 80
    Public SWL_lvl1_width As Integer = 52
    Public SWL_lvl1_height As Integer = 239
    Public SWR_lvl1_width As Integer = 52
    Public SWR_lvl1_height As Integer = 239
    Public SWL_lvl2_width As Integer = 47
    Public SWL_lvl2_height As Integer = 195
    Public SWR_lvl2_width As Integer = 47
    Public SWR_lvl2_height As Integer = 195
    Public SWL_lvl3_width As Integer = 30
    Public SWL_lvl3_height As Integer = 118
    Public SWL_lvl4_width As Integer = 6
    Public SWL_lvl4_height As Integer = 84


    'Main walls positions
    Public MW_lvl1_xpos As Integer = 48
    Public MW_lvl1_ypos As Integer = 14

    Public MW_lvl2_xpos As Integer = 97
    Public MW_lvl2_ypos As Integer = 40

    Public MW_lvl3_xpos As Integer = 130
    Public MW_lvl3_ypos As Integer = 52

    'Sidewalls level 1
    Public SWL_lvl1_xpos As Integer = 0
    Public SWL_lvl1_ypos As Integer = 0
    Public SWR_lvl1_xpos As Integer = 303
    Public SWR_lvl1_ypos As Integer = 0

    'Sidewalls level 2
    Public SWL_lvl2_xpos As Integer = 49 '51
    Public SWL_lvl2_ypos As Integer = 13
    Public SWR_lvl2_xpos As Integer = 259
    Public SWR_lvl2_ypos As Integer = 13

    'Sidewalls level 3
    Public SWL_lvl3_xpos As Integer = 95 '97
    Public SWL_lvl3_ypos As Integer = 38
    Public SWR_lvl3_xpos As Integer = 229
    Public SWR_lvl3_ypos As Integer = 38

    'Sidewalls level 4
    Public SWL_lvl4_xpos As Integer = 127
    Public SWL_lvl4_ypos As Integer = 50
    Public SWR_lvl4_xpos As Integer = 223
    Public SWR_lvl4_ypos As Integer = 50

    'New Control Positions
    Public Text_xpos As Integer
    Public Text_ypos As Integer
    Public Text_width As Integer
    Public Text_height As Integer

    Public dir_leftturn_xpos As Integer = 372
    Public dir_leftturn_ypos As Integer = 5
    Public dir_width As Integer = 60
    Public dir_height As Integer = 13

    Public leftturn_xpos As Integer = 0 '143
    Public leftturn_ypos As Integer = 0 '249
    Public leftturn_width As Integer = 0 '46
    Public leftturn_height As Integer = 0 '38
    Public left_xpos As Integer = 0 '143
    Public left_ypos As Integer = 0 '287
    Public up_xpos As Integer = 0 '191
    Public up_ypos As Integer = 0 '249
    Public down_xpos As Integer = 0 '191
    Public down_ypos As Integer = 0 '287
    Public rightturn_xpos As Integer = 0 '239
    Public rightturn_ypos As Integer = 0 '249
    Public right_xpos As Integer = 0 '239
    Public right_ypos As Integer = 0 '287

    'Main Character Positions
    Public names_xpos As Integer = 0 '10
    Public names_ypos As Integer = 0 '5
    Public names_width As Integer = 0 '60
    Public names_height As Integer = 0 '13
    Public names_horizontal As Integer = 0 '500
    Public names_vertical As Integer = 0 '103

    Public portrait_xpos As Integer = 0
    Public portrait_ypos As Integer = 0 '25
    Public portrait_width As Integer = 0 '63
    Public portrait_height As Integer = 0 '59
    Public portrait_horizontal As Integer = 0 '560
    Public portrait_vertical As Integer = 0 '102

    Public hand1pic_xpos As Integer = 0 '85
    Public hand1pic_ypos As Integer = 0 '22
    Public hand1pic_width As Integer = 0 '30
    Public hand1pic_height As Integer = 0 '30
    Public hand1pic_horizontal As Integer = 0 '434
    Public hand1pic_vertical As Integer = 0 '103

    Public hand2pic_xpos As Integer = 0 '85
    Public hand2pic_ypos As Integer = 0 '54
    Public hand2pic_width As Integer = 0 '30
    Public hand2pic_height As Integer = 0 '30
    Public hand2pic_horizontal As Integer = 0 '434
    Public hand2pic_vertical As Integer = 0 '103

    Public healthpic_xpos As Integer = 5
    Public healthpic_ypos As Integer = 88
    Public healthpic_width As Integer = 123
    Public healthpic_height As Integer = 10
    Public healthpic_horizontal As Integer = 500
    Public healthpic_vertical As Integer = 104

    Public manapic_xpos As Integer = 432
    Public manapic_ypos As Integer = 89 '82
    Public manapic_width As Integer = 0 '62
    Public manapic_height As Integer = 0 '8
    Public manapic_horizontal As Integer = 146
    Public manapic_vertical As Integer = 94

    'Character Inventory Positions
    Public inv_xpos As Integer = 0 '369
    Public inv_ypos As Integer = 0 '74
    Public inv_width As Integer = 0 '35
    Public inv_height As Integer = 0 '34
    Public inv_horizontal As Integer = 0 '35
    Public inv_vertical As Integer = 0 '34

    Public invuser_xpos As Integer = 0 '368
    Public invuser_ypos As Integer = 0 '5
    Public invuser_width As Integer = 0 '69
    Public invuser_height As Integer = 0 '63
    Public invname_xpos As Integer = 0 '504
    Public invname_ypos As Integer = 0 '27
    Public invname_width As Integer = 0 '100
    Public invname_height As Integer = 0 '17

    Public healthmonster_xpos As Integer = 177 '177 '35
    Public healthmonster_ypos As Integer = 220 '192
    Public healthmonster_width As Integer = 300 '300
    Public healthmonster_height As Integer = 10 '10
    Public monster_wounds_xpos As Integer = 145 '140
    Public monster_wounds_ypos As Integer = 130
    Public monster_text_xpos As Integer = 175 '170
    Public monster_text_ypos As Integer = 140

    Public CampButton_xpos As Integer = 576 '35
    Public CampButton_ypos As Integer = 354
    Public CampButton_width As Integer = 80
    Public CampButton_height As Integer = 45



End Module

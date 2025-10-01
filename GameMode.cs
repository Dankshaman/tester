using System;
using System.Collections;
using System.Collections.Generic;
using NewNet;
using UnityEngine;

// Token: 0x02000115 RID: 277
public class GameMode : NetworkSingleton<GameMode>
{
	// Token: 0x17000226 RID: 550
	// (get) Token: 0x06000E46 RID: 3654 RVA: 0x0005C4E9 File Offset: 0x0005A6E9
	public GameObject D2
	{
		get
		{
			return this.GetPrefab("Quarter");
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06000E47 RID: 3655 RVA: 0x0005C4F6 File Offset: 0x0005A6F6
	public GameObject D4
	{
		get
		{
			return this.GetPrefab("Die_4");
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06000E48 RID: 3656 RVA: 0x0005C503 File Offset: 0x0005A703
	public GameObject D6
	{
		get
		{
			return this.GetPrefab("Die_6");
		}
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06000E49 RID: 3657 RVA: 0x0005C510 File Offset: 0x0005A710
	public GameObject D6Rounded
	{
		get
		{
			return this.GetPrefab("Die_6_Rounded");
		}
	}

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06000E4A RID: 3658 RVA: 0x0005C51D File Offset: 0x0005A71D
	public GameObject D8
	{
		get
		{
			return this.GetPrefab("Die_8");
		}
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06000E4B RID: 3659 RVA: 0x0005C52A File Offset: 0x0005A72A
	public GameObject D10
	{
		get
		{
			return this.GetPrefab("Die_10");
		}
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06000E4C RID: 3660 RVA: 0x0005C537 File Offset: 0x0005A737
	public GameObject D12
	{
		get
		{
			return this.GetPrefab("Die_12");
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06000E4D RID: 3661 RVA: 0x0005C544 File Offset: 0x0005A744
	public GameObject D20
	{
		get
		{
			return this.GetPrefab("Die_20");
		}
	}

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06000E4E RID: 3662 RVA: 0x0005C551 File Offset: 0x0005A751
	public GameObject Card
	{
		get
		{
			return this.GetPrefab("Card");
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06000E4F RID: 3663 RVA: 0x0005C55E File Offset: 0x0005A75E
	public GameObject Deck
	{
		get
		{
			return this.GetPrefab("Deck");
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06000E50 RID: 3664 RVA: 0x0005C56B File Offset: 0x0005A76B
	public GameObject DeckCustom
	{
		get
		{
			return this.GetPrefab("DeckCustom");
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06000E51 RID: 3665 RVA: 0x0005C578 File Offset: 0x0005A778
	public GameObject DeckCardBotMain
	{
		get
		{
			return this.GetPrefab("Deck_CardBot_Main");
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06000E52 RID: 3666 RVA: 0x0005C585 File Offset: 0x0005A785
	public GameObject DeckCardBotHead
	{
		get
		{
			return this.GetPrefab("Deck_CardBot_Head");
		}
	}

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06000E53 RID: 3667 RVA: 0x0005C592 File Offset: 0x0005A792
	public GameObject Poker10
	{
		get
		{
			return this.GetPrefab("Chip_10");
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06000E54 RID: 3668 RVA: 0x0005C59F File Offset: 0x0005A79F
	public GameObject Poker50
	{
		get
		{
			return this.GetPrefab("Chip_50");
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06000E55 RID: 3669 RVA: 0x0005C5AC File Offset: 0x0005A7AC
	public GameObject Poker100
	{
		get
		{
			return this.GetPrefab("Chip_100");
		}
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06000E56 RID: 3670 RVA: 0x0005C5B9 File Offset: 0x0005A7B9
	public GameObject Poker500
	{
		get
		{
			return this.GetPrefab("Chip_500");
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0005C5C6 File Offset: 0x0005A7C6
	public GameObject Poker1000
	{
		get
		{
			return this.GetPrefab("Chip_1000");
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06000E58 RID: 3672 RVA: 0x0005C5D3 File Offset: 0x0005A7D3
	public GameObject PokerStack
	{
		get
		{
			return this.GetPrefab("ChipStack");
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0005C5E0 File Offset: 0x0005A7E0
	public GameObject ChessPawn
	{
		get
		{
			return this.GetPrefab("Chess_Pawn");
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06000E5A RID: 3674 RVA: 0x0005C5ED File Offset: 0x0005A7ED
	public GameObject ChessRook
	{
		get
		{
			return this.GetPrefab("Chess_Rook");
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06000E5B RID: 3675 RVA: 0x0005C5FA File Offset: 0x0005A7FA
	public GameObject ChessKnight
	{
		get
		{
			return this.GetPrefab("Chess_Knight");
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06000E5C RID: 3676 RVA: 0x0005C607 File Offset: 0x0005A807
	public GameObject ChessBishop
	{
		get
		{
			return this.GetPrefab("Chess_Bishop");
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06000E5D RID: 3677 RVA: 0x0005C614 File Offset: 0x0005A814
	public GameObject ChessQueen
	{
		get
		{
			return this.GetPrefab("Chess_Queen");
		}
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06000E5E RID: 3678 RVA: 0x0005C621 File Offset: 0x0005A821
	public GameObject ChessKing
	{
		get
		{
			return this.GetPrefab("Chess_King");
		}
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06000E5F RID: 3679 RVA: 0x0005C62E File Offset: 0x0005A82E
	public GameObject CheckerWhite
	{
		get
		{
			return this.GetPrefab("Checker_White");
		}
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06000E60 RID: 3680 RVA: 0x0005C63B File Offset: 0x0005A83B
	public GameObject CheckerBlack
	{
		get
		{
			return this.GetPrefab("Checker_Black");
		}
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06000E61 RID: 3681 RVA: 0x0005C648 File Offset: 0x0005A848
	public GameObject CheckerRed
	{
		get
		{
			return this.GetPrefab("Checker_Red");
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06000E62 RID: 3682 RVA: 0x0005C655 File Offset: 0x0005A855
	public GameObject CheckerStack
	{
		get
		{
			return this.GetPrefab("CheckerStack");
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06000E63 RID: 3683 RVA: 0x0005C662 File Offset: 0x0005A862
	public GameObject Domino
	{
		get
		{
			return this.GetPrefab("Domino");
		}
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06000E64 RID: 3684 RVA: 0x0005C66F File Offset: 0x0005A86F
	public GameObject ReversiChip
	{
		get
		{
			return this.GetPrefab("reversi_chip");
		}
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06000E65 RID: 3685 RVA: 0x0005C67C File Offset: 0x0005A87C
	public GameObject BackgammonPieceWhite
	{
		get
		{
			return this.GetPrefab("backgammon_piece_white");
		}
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06000E66 RID: 3686 RVA: 0x0005C689 File Offset: 0x0005A889
	public GameObject BackgammonPieceBrown
	{
		get
		{
			return this.GetPrefab("backgammon_piece_brown");
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06000E67 RID: 3687 RVA: 0x0005C696 File Offset: 0x0005A896
	public GameObject ChineseCheckersPiece
	{
		get
		{
			return this.GetPrefab("Chinese_Checkers_Piece");
		}
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06000E68 RID: 3688 RVA: 0x0005C6A3 File Offset: 0x0005A8A3
	public GameObject GoPieceWhite
	{
		get
		{
			return this.GetPrefab("go_game_piece_white");
		}
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06000E69 RID: 3689 RVA: 0x0005C6B0 File Offset: 0x0005A8B0
	public GameObject GoPieceBlack
	{
		get
		{
			return this.GetPrefab("go_game_piece_black");
		}
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06000E6A RID: 3690 RVA: 0x0005C6BD File Offset: 0x0005A8BD
	public GameObject GoBowlWhite
	{
		get
		{
			return this.GetPrefab("go_game_bowl_white");
		}
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06000E6B RID: 3691 RVA: 0x0005C6CA File Offset: 0x0005A8CA
	public GameObject GoBowlBlack
	{
		get
		{
			return this.GetPrefab("go_game_bowl_black");
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06000E6C RID: 3692 RVA: 0x0005C6D7 File Offset: 0x0005A8D7
	public GameObject BlockSquare
	{
		get
		{
			return this.GetPrefab("BlockSquare");
		}
	}

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06000E6D RID: 3693 RVA: 0x0005C6E4 File Offset: 0x0005A8E4
	public GameObject BlockRectangle
	{
		get
		{
			return this.GetPrefab("BlockRectangle");
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06000E6E RID: 3694 RVA: 0x0005C6F1 File Offset: 0x0005A8F1
	public GameObject BlockTriangle
	{
		get
		{
			return this.GetPrefab("BlockTriangle");
		}
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06000E6F RID: 3695 RVA: 0x0005C6FE File Offset: 0x0005A8FE
	public GameObject PlayerPawn
	{
		get
		{
			return this.GetPrefab("PlayerPawn");
		}
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06000E70 RID: 3696 RVA: 0x0005C70B File Offset: 0x0005A90B
	public GameObject MetalBall
	{
		get
		{
			return this.GetPrefab("Metal Ball");
		}
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0005C718 File Offset: 0x0005A918
	public GameObject DiePiecepack
	{
		get
		{
			return this.GetPrefab("Die_Piecepack");
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06000E72 RID: 3698 RVA: 0x0005C725 File Offset: 0x0005A925
	public GameObject PiecepackSuns
	{
		get
		{
			return this.GetPrefab("PiecePack_Suns");
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06000E73 RID: 3699 RVA: 0x0005C732 File Offset: 0x0005A932
	public GameObject PiecepackMoons
	{
		get
		{
			return this.GetPrefab("PiecePack_Moons");
		}
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06000E74 RID: 3700 RVA: 0x0005C73F File Offset: 0x0005A93F
	public GameObject PiecepackCrowns
	{
		get
		{
			return this.GetPrefab("PiecePack_Crowns");
		}
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06000E75 RID: 3701 RVA: 0x0005C74C File Offset: 0x0005A94C
	public GameObject PiecepackArms
	{
		get
		{
			return this.GetPrefab("PiecePack_Arms");
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06000E76 RID: 3702 RVA: 0x0005C759 File Offset: 0x0005A959
	public GameObject MahjongTile
	{
		get
		{
			return this.GetPrefab("Mahjong_Tile");
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06000E77 RID: 3703 RVA: 0x0005C766 File Offset: 0x0005A966
	public GameObject MahjongCoin
	{
		get
		{
			return this.GetPrefab("Mahjong_Coin");
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06000E78 RID: 3704 RVA: 0x0005C773 File Offset: 0x0005A973
	public GameObject MahjongStick
	{
		get
		{
			return this.GetPrefab("Mahjong_Stick");
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06000E79 RID: 3705 RVA: 0x0005C780 File Offset: 0x0005A980
	public GameObject Bag
	{
		get
		{
			return this.GetPrefab("Bag");
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06000E7A RID: 3706 RVA: 0x0005C78D File Offset: 0x0005A98D
	public GameObject rpgORC
	{
		get
		{
			return this.GetPrefab("rpg_ORC");
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06000E7B RID: 3707 RVA: 0x0005C79A File Offset: 0x0005A99A
	public GameObject rpgBEAR
	{
		get
		{
			return this.GetPrefab("rpg_BEAR");
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06000E7C RID: 3708 RVA: 0x0005C7A7 File Offset: 0x0005A9A7
	public GameObject rpgTROLL
	{
		get
		{
			return this.GetPrefab("rpg_TROLL");
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06000E7D RID: 3709 RVA: 0x0005C7B4 File Offset: 0x0005A9B4
	public GameObject rpgEVILWATCHER
	{
		get
		{
			return this.GetPrefab("rpg_EVIL_WATCHER");
		}
	}

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06000E7E RID: 3710 RVA: 0x0005C7C1 File Offset: 0x0005A9C1
	public GameObject rpgGIANTVIPER
	{
		get
		{
			return this.GetPrefab("rpg_GIANT_VIPER");
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06000E7F RID: 3711 RVA: 0x0005C7CE File Offset: 0x0005A9CE
	public GameObject rpgGOBLIN
	{
		get
		{
			return this.GetPrefab("rpg_GOBLIN");
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06000E80 RID: 3712 RVA: 0x0005C7DB File Offset: 0x0005A9DB
	public GameObject rpgGOLEM
	{
		get
		{
			return this.GetPrefab("rpg_GOLEM");
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06000E81 RID: 3713 RVA: 0x0005C7E8 File Offset: 0x0005A9E8
	public GameObject rpgGRIFFON
	{
		get
		{
			return this.GetPrefab("rpg_GRIFFON");
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0005C7F5 File Offset: 0x0005A9F5
	public GameObject rpgHYDRA
	{
		get
		{
			return this.GetPrefab("rpg_HYDRA");
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06000E83 RID: 3715 RVA: 0x0005C802 File Offset: 0x0005AA02
	public GameObject rpgMUMMY
	{
		get
		{
			return this.GetPrefab("rpg_MUMMY");
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0005C80F File Offset: 0x0005AA0F
	public GameObject rpgVAMPIRE
	{
		get
		{
			return this.GetPrefab("rpg_VAMPIRE");
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0005C81C File Offset: 0x0005AA1C
	public GameObject rpgRAT
	{
		get
		{
			return this.GetPrefab("rpg_RAT");
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0005C829 File Offset: 0x0005AA29
	public GameObject rpgTREEENT
	{
		get
		{
			return this.GetPrefab("rpg_TREE_ENT");
		}
	}

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0005C836 File Offset: 0x0005AA36
	public GameObject rpgWOLF
	{
		get
		{
			return this.GetPrefab("rpg_WOLF");
		}
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0005C843 File Offset: 0x0005AA43
	public GameObject rpgWEREWOLF
	{
		get
		{
			return this.GetPrefab("rpg_WEREWOLF");
		}
	}

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0005C850 File Offset: 0x0005AA50
	public GameObject rpgLIZARDWARRIOR
	{
		get
		{
			return this.GetPrefab("rpg_LIZARD_WARRIOR");
		}
	}

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0005C85D File Offset: 0x0005AA5D
	public GameObject rpgDRAGONIDE
	{
		get
		{
			return this.GetPrefab("rpg_DRAGONIDE");
		}
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0005C86A File Offset: 0x0005AA6A
	public GameObject rpgGHOUL
	{
		get
		{
			return this.GetPrefab("rpg_GHOUL");
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06000E8C RID: 3724 RVA: 0x0005C877 File Offset: 0x0005AA77
	public GameObject rpgOGRE
	{
		get
		{
			return this.GetPrefab("rpg_OGRE");
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06000E8D RID: 3725 RVA: 0x0005C884 File Offset: 0x0005AA84
	public GameObject rpgCHIMERA
	{
		get
		{
			return this.GetPrefab("rpg_CHIMERA");
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06000E8E RID: 3726 RVA: 0x0005C891 File Offset: 0x0005AA91
	public GameObject rpgWYVERN
	{
		get
		{
			return this.GetPrefab("rpg_WYVERN");
		}
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06000E8F RID: 3727 RVA: 0x0005C89E File Offset: 0x0005AA9E
	public GameObject rpgCYCLOPS
	{
		get
		{
			return this.GetPrefab("rpg_CYCLOP");
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06000E90 RID: 3728 RVA: 0x0005C8AB File Offset: 0x0005AAAB
	public GameObject rpgKOBOLD
	{
		get
		{
			return this.GetPrefab("rpg_KOBOLD");
		}
	}

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06000E91 RID: 3729 RVA: 0x0005C8B8 File Offset: 0x0005AAB8
	public GameObject rpgSKELETONKNIGHT
	{
		get
		{
			return this.GetPrefab("rpg_SKELETON_KNIGHT");
		}
	}

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06000E92 RID: 3730 RVA: 0x0005C8C5 File Offset: 0x0005AAC5
	public GameObject rpgMANTICORA
	{
		get
		{
			return this.GetPrefab("rpg_MANTICORA");
		}
	}

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06000E93 RID: 3731 RVA: 0x0005C8D2 File Offset: 0x0005AAD2
	public GameObject rpgBARGHEST
	{
		get
		{
			return this.GetPrefab("rpg_BARGHEST");
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06000E94 RID: 3732 RVA: 0x0005C8DF File Offset: 0x0005AADF
	public GameObject rpgBASILISK
	{
		get
		{
			return this.GetPrefab("rpg_BASILISK");
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06000E95 RID: 3733 RVA: 0x0005C8EC File Offset: 0x0005AAEC
	public GameObject rpgBLACK_DRAGON
	{
		get
		{
			return this.GetPrefab("rpg_BLACK_DRAGON");
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06000E96 RID: 3734 RVA: 0x0005C8F9 File Offset: 0x0005AAF9
	public GameObject rpgCENTAUR
	{
		get
		{
			return this.GetPrefab("rpg_CENTAUR");
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06000E97 RID: 3735 RVA: 0x0005C906 File Offset: 0x0005AB06
	public GameObject rpgCERBERUS
	{
		get
		{
			return this.GetPrefab("rpg_CERBERUS");
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06000E98 RID: 3736 RVA: 0x0005C913 File Offset: 0x0005AB13
	public GameObject rpgCRASC
	{
		get
		{
			return this.GetPrefab("rpg_CRASC");
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06000E99 RID: 3737 RVA: 0x0005C920 File Offset: 0x0005AB20
	public GameObject rpgDARKNESS_WARLORD
	{
		get
		{
			return this.GetPrefab("rpg_DARKNESS_WARLORD");
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06000E9A RID: 3738 RVA: 0x0005C92D File Offset: 0x0005AB2D
	public GameObject rpgKNIGHT
	{
		get
		{
			return this.GetPrefab("rpg_KNIGHT");
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06000E9B RID: 3739 RVA: 0x0005C93A File Offset: 0x0005AB3A
	public GameObject rpgWARRIOR
	{
		get
		{
			return this.GetPrefab("rpg_WARRIOR");
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06000E9C RID: 3740 RVA: 0x0005C947 File Offset: 0x0005AB47
	public GameObject rpgMAGE
	{
		get
		{
			return this.GetPrefab("rpg_MAGE");
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06000E9D RID: 3741 RVA: 0x0005C954 File Offset: 0x0005AB54
	public GameObject rpgRANGER
	{
		get
		{
			return this.GetPrefab("rpg_RANGER");
		}
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06000E9E RID: 3742 RVA: 0x0005C961 File Offset: 0x0005AB61
	public GameObject rpgTHIEF
	{
		get
		{
			return this.GetPrefab("rpg_THIEF");
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06000E9F RID: 3743 RVA: 0x0005C96E File Offset: 0x0005AB6E
	public GameObject FigurineLoin
	{
		get
		{
			return this.GetPrefab("Figurine_Sir_Loin");
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x0005C97B File Offset: 0x0005AB7B
	public GameObject FigurineCardBot
	{
		get
		{
			return this.GetPrefab("Figurine_Card_Bot");
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06000EA1 RID: 3745 RVA: 0x0005C988 File Offset: 0x0005AB88
	public GameObject FigurineKimiKats
	{
		get
		{
			return this.GetPrefab("Figurine_Kimi_Kat");
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x0005C995 File Offset: 0x0005AB95
	public GameObject FigurineKnil
	{
		get
		{
			return this.GetPrefab("Figurine_Knil");
		}
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06000EA3 RID: 3747 RVA: 0x0005C9A2 File Offset: 0x0005ABA2
	public GameObject FigurineMara
	{
		get
		{
			return this.GetPrefab("Figurine_Mara");
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06000EA4 RID: 3748 RVA: 0x0005C9AF File Offset: 0x0005ABAF
	public GameObject FigurineZomblor
	{
		get
		{
			return this.GetPrefab("Figurine_Zomblor");
		}
	}

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06000EA5 RID: 3749 RVA: 0x0005C9BC File Offset: 0x0005ABBC
	public GameObject FigurineZeke
	{
		get
		{
			return this.GetPrefab("Figurine_Zeke");
		}
	}

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06000EA6 RID: 3750 RVA: 0x0005C9C9 File Offset: 0x0005ABC9
	public GameObject CustomFigurine
	{
		get
		{
			return this.GetPrefab("Figurine_Custom");
		}
	}

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06000EA7 RID: 3751 RVA: 0x0005C9D6 File Offset: 0x0005ABD6
	public GameObject TilesetFloor
	{
		get
		{
			return this.GetPrefab("Tileset_Floor");
		}
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06000EA8 RID: 3752 RVA: 0x0005C9E3 File Offset: 0x0005ABE3
	public GameObject TilesetWall
	{
		get
		{
			return this.GetPrefab("Tileset_Wall");
		}
	}

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06000EA9 RID: 3753 RVA: 0x0005C9F0 File Offset: 0x0005ABF0
	public GameObject TilesetCorner
	{
		get
		{
			return this.GetPrefab("Tileset_Corner");
		}
	}

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06000EAA RID: 3754 RVA: 0x0005C9FD File Offset: 0x0005ABFD
	public GameObject TilesetChair
	{
		get
		{
			return this.GetPrefab("Tileset_Chair");
		}
	}

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06000EAB RID: 3755 RVA: 0x0005CA0A File Offset: 0x0005AC0A
	public GameObject TilesetTable
	{
		get
		{
			return this.GetPrefab("Tileset_Table");
		}
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06000EAC RID: 3756 RVA: 0x0005CA17 File Offset: 0x0005AC17
	public GameObject TilesetBarrel
	{
		get
		{
			return this.GetPrefab("Tileset_Barrel");
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06000EAD RID: 3757 RVA: 0x0005CA24 File Offset: 0x0005AC24
	public GameObject TilesetChest
	{
		get
		{
			return this.GetPrefab("Tileset_Chest");
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06000EAE RID: 3758 RVA: 0x0005CA31 File Offset: 0x0005AC31
	public GameObject TilesetTree
	{
		get
		{
			return this.GetPrefab("Tileset_Tree");
		}
	}

	// Token: 0x1700028F RID: 655
	// (get) Token: 0x06000EAF RID: 3759 RVA: 0x0005CA3E File Offset: 0x0005AC3E
	public GameObject TilesetRock
	{
		get
		{
			return this.GetPrefab("Tileset_Rock");
		}
	}

	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x0005CA4B File Offset: 0x0005AC4B
	public GameObject CustomModel
	{
		get
		{
			return this.GetPrefab("Custom_Model");
		}
	}

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06000EB1 RID: 3761 RVA: 0x0005CA58 File Offset: 0x0005AC58
	public GameObject CustomModelStack
	{
		get
		{
			return this.GetPrefab("Custom_Model_Stack");
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06000EB2 RID: 3762 RVA: 0x0005CA65 File Offset: 0x0005AC65
	public GameObject CustomModelBag
	{
		get
		{
			return this.GetPrefab("Custom_Model_Bag");
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06000EB3 RID: 3763 RVA: 0x0005CA72 File Offset: 0x0005AC72
	public GameObject CustomModelInfiniteBag
	{
		get
		{
			return this.GetPrefab("Custom_Model_Infinite_Bag");
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06000EB4 RID: 3764 RVA: 0x0005CA7F File Offset: 0x0005AC7F
	public GameObject CustomAssetbundle
	{
		get
		{
			return this.GetPrefab("Custom_Assetbundle");
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06000EB5 RID: 3765 RVA: 0x0005CA8C File Offset: 0x0005AC8C
	public GameObject CustomAssetbundleStack
	{
		get
		{
			return this.GetPrefab("Custom_Assetbundle_Stack");
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06000EB6 RID: 3766 RVA: 0x0005CA99 File Offset: 0x0005AC99
	public GameObject CustomAssetbundleBag
	{
		get
		{
			return this.GetPrefab("Custom_Assetbundle_Bag");
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x0005CAA6 File Offset: 0x0005ACA6
	public GameObject CustomAssetbundleInfiniteBag
	{
		get
		{
			return this.GetPrefab("Custom_Assetbundle_Infinite_Bag");
		}
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x0005CAB3 File Offset: 0x0005ACB3
	public GameObject SearchTriggerObject
	{
		get
		{
			return this.GetPrefab("SearchTriggerObject");
		}
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06000EB9 RID: 3769 RVA: 0x0005CAC0 File Offset: 0x0005ACC0
	public GameObject SearchSpaceHolder
	{
		get
		{
			return this.GetPrefab("SearchSpaceHolder");
		}
	}

	// Token: 0x1700029A RID: 666
	// (get) Token: 0x06000EBA RID: 3770 RVA: 0x0005CACD File Offset: 0x0005ACCD
	public GameObject FogOfWarTrigger
	{
		get
		{
			return this.GetPrefab("FogOfWarTrigger");
		}
	}

	// Token: 0x1700029B RID: 667
	// (get) Token: 0x06000EBB RID: 3771 RVA: 0x0005CADA File Offset: 0x0005ACDA
	public GameObject TextSelection
	{
		get
		{
			return this.GetPrefab("TextSelection");
		}
	}

	// Token: 0x1700029C RID: 668
	// (get) Token: 0x06000EBC RID: 3772 RVA: 0x0005CAE7 File Offset: 0x0005ACE7
	public GameObject TextLabel
	{
		get
		{
			return this.GetPrefab("3DText");
		}
	}

	// Token: 0x1700029D RID: 669
	// (get) Token: 0x06000EBD RID: 3773 RVA: 0x0005CAF4 File Offset: 0x0005ACF4
	public GameObject RandomizeTrigger
	{
		get
		{
			return this.GetPrefab("RandomizeTrigger");
		}
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x06000EBE RID: 3774 RVA: 0x0005CB01 File Offset: 0x0005AD01
	public GameObject ScriptingTrigger
	{
		get
		{
			return this.GetPrefab("ScriptingTrigger");
		}
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x06000EBF RID: 3775 RVA: 0x0005CB0E File Offset: 0x0005AD0E
	public GameObject FogOfWar
	{
		get
		{
			return this.GetPrefab("FogOfWar");
		}
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x06000EC0 RID: 3776 RVA: 0x0005CB1B File Offset: 0x0005AD1B
	public GameObject LayoutZone
	{
		get
		{
			return this.GetPrefab("LayoutZone");
		}
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x06000EC1 RID: 3777 RVA: 0x0005CB28 File Offset: 0x0005AD28
	public GameObject DigitalClock
	{
		get
		{
			return this.GetPrefab("Digital_Clock");
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x06000EC2 RID: 3778 RVA: 0x0005CB35 File Offset: 0x0005AD35
	public GameObject Tablet
	{
		get
		{
			return this.GetPrefab("Tablet");
		}
	}

	// Token: 0x170002A3 RID: 675
	// (get) Token: 0x06000EC3 RID: 3779 RVA: 0x0005CB42 File Offset: 0x0005AD42
	public GameObject SnapPoint
	{
		get
		{
			if (!VRHMD.isVR)
			{
				return this.GetPrefab("Snap_Point");
			}
			return this.GetPrefab("Snap_Point_VR");
		}
	}

	// Token: 0x170002A4 RID: 676
	// (get) Token: 0x06000EC4 RID: 3780 RVA: 0x0005C70B File Offset: 0x0005A90B
	public GameObject Test
	{
		get
		{
			return this.GetPrefab("Metal Ball");
		}
	}

	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06000EC5 RID: 3781 RVA: 0x0005CB62 File Offset: 0x0005AD62
	public GameObject JigsawPuzzlePiece20
	{
		get
		{
			return this.GetPrefab("JigsawPuzzlePiece20");
		}
	}

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06000EC6 RID: 3782 RVA: 0x0005CB6F File Offset: 0x0005AD6F
	public GameObject JigsawPuzzlePiece80
	{
		get
		{
			return this.GetPrefab("JigsawPuzzlePiece80");
		}
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x06000EC7 RID: 3783 RVA: 0x0005CB7C File Offset: 0x0005AD7C
	public GameObject JigsawPuzzlePiece180
	{
		get
		{
			return this.GetPrefab("JigsawPuzzlePiece180");
		}
	}

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x06000EC8 RID: 3784 RVA: 0x0005CB89 File Offset: 0x0005AD89
	public GameObject JigsawPuzzlePiece320
	{
		get
		{
			return this.GetPrefab("JigsawPuzzlePiece320");
		}
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x06000EC9 RID: 3785 RVA: 0x0005CB96 File Offset: 0x0005AD96
	public GameObject JigsawPuzzleBox
	{
		get
		{
			return this.GetPrefab("JigsawPuzzleBox");
		}
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x06000ECA RID: 3786 RVA: 0x0005CBA3 File Offset: 0x0005ADA3
	public GameObject Sky_Museum
	{
		get
		{
			return this.GetPrefab("Sky_Museum");
		}
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x06000ECB RID: 3787 RVA: 0x0005CBB0 File Offset: 0x0005ADB0
	public GameObject Sky_Field
	{
		get
		{
			return this.GetPrefab("Sky_Field");
		}
	}

	// Token: 0x170002AC RID: 684
	// (get) Token: 0x06000ECC RID: 3788 RVA: 0x0005CBBD File Offset: 0x0005ADBD
	public GameObject Sky_Forest
	{
		get
		{
			return this.GetPrefab("Sky_Forest");
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06000ECD RID: 3789 RVA: 0x0005CBCA File Offset: 0x0005ADCA
	public GameObject Sky_Tunnel
	{
		get
		{
			return this.GetPrefab("Sky_Tunnel");
		}
	}

	// Token: 0x170002AE RID: 686
	// (get) Token: 0x06000ECE RID: 3790 RVA: 0x0005CBD7 File Offset: 0x0005ADD7
	public GameObject Sky_Cathedral
	{
		get
		{
			return this.GetPrefab("Sky_Cathedral");
		}
	}

	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06000ECF RID: 3791 RVA: 0x0005CBE4 File Offset: 0x0005ADE4
	public GameObject Sky_Downtown
	{
		get
		{
			return this.GetPrefab("Sky_Downtown");
		}
	}

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06000ED0 RID: 3792 RVA: 0x0005CBF1 File Offset: 0x0005ADF1
	public GameObject Sky_Regal
	{
		get
		{
			return this.GetPrefab("Sky_Regal");
		}
	}

	// Token: 0x170002B1 RID: 689
	// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x0005CBFE File Offset: 0x0005ADFE
	public GameObject Sky_Sunset
	{
		get
		{
			return this.GetPrefab("Sky_Sunset");
		}
	}

	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x0005CC0B File Offset: 0x0005AE0B
	public GameObject Sky_Custom
	{
		get
		{
			return this.GetPrefab("Sky_Custom");
		}
	}

	// Token: 0x170002B3 RID: 691
	// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x0005CC18 File Offset: 0x0005AE18
	public GameObject HexagonTable
	{
		get
		{
			return this.GetPrefab("Table_Hexagon");
		}
	}

	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x0005CC25 File Offset: 0x0005AE25
	public GameObject OctagonTable
	{
		get
		{
			return this.GetPrefab("Table_Octagon");
		}
	}

	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x0005CC32 File Offset: 0x0005AE32
	public GameObject SquareTable
	{
		get
		{
			return this.GetPrefab("Table_Square");
		}
	}

	// Token: 0x170002B6 RID: 694
	// (get) Token: 0x06000ED6 RID: 3798 RVA: 0x0005CC3F File Offset: 0x0005AE3F
	public GameObject PokerTable
	{
		get
		{
			return this.GetPrefab("Table_Poker");
		}
	}

	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06000ED7 RID: 3799 RVA: 0x0005CC4C File Offset: 0x0005AE4C
	public GameObject RPGTable
	{
		get
		{
			return this.GetPrefab("Table_RPG");
		}
	}

	// Token: 0x170002B8 RID: 696
	// (get) Token: 0x06000ED8 RID: 3800 RVA: 0x0005CC59 File Offset: 0x0005AE59
	public GameObject CircleTable
	{
		get
		{
			return this.GetPrefab("Table_Circular");
		}
	}

	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06000ED9 RID: 3801 RVA: 0x0005CC66 File Offset: 0x0005AE66
	public GameObject CustomTable
	{
		get
		{
			return this.GetPrefab("Table_Custom");
		}
	}

	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06000EDA RID: 3802 RVA: 0x0005CC73 File Offset: 0x0005AE73
	public GameObject GlassTable
	{
		get
		{
			return this.GetPrefab("Table_Glass");
		}
	}

	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06000EDB RID: 3803 RVA: 0x0005CC80 File Offset: 0x0005AE80
	public GameObject HandTrigger
	{
		get
		{
			return this.GetPrefab("HandTrigger");
		}
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x0005CC8D File Offset: 0x0005AE8D
	public GameObject GetPrefab(string name)
	{
		return (GameObject)Resources.Load(name);
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x0005CC9C File Offset: 0x0005AE9C
	public void Spawn(string GameName)
	{
		if (NetworkSingleton<ManagerPhysicsObject>.Instance.Table.GetComponent<CustomImage>() && DLCManager.URLisDLC(NetworkSingleton<ManagerPhysicsObject>.Instance.Table.GetComponent<CustomImage>().CustomImageURL))
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.ChangeTable(NetworkSingleton<ManagerPhysicsObject>.Instance.Table.name);
		}
		if (CustomSky.ActiveCustomSky && DLCManager.URLisDLC(CustomSky.ActiveCustomSky.CustomSkyURL))
		{
			NetworkSingleton<ManagerPhysicsObject>.Instance.DestroyThisObject(CustomSky.ActiveCustomSky);
		}
		Transform transform = base.transform.Find(GameName);
		if (!transform)
		{
			Debug.Log("Game not found");
			return;
		}
		GameObject gameObject = transform.gameObject;
		if (GameName == "Game_Tools")
		{
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				Transform child = gameObject.transform.GetChild(i);
				if (child)
				{
					string text = child.name;
					int num = -1;
					int num2 = -1;
					int num3 = -1;
					if (text.Contains("?mat="))
					{
						string[] array = text.Split(new string[]
						{
							"?mat="
						}, StringSplitOptions.RemoveEmptyEntries);
						text = array[0];
						num = int.Parse(array[1]);
					}
					if (text.Contains("?mesh="))
					{
						string[] array2 = text.Split(new string[]
						{
							"?mesh="
						}, StringSplitOptions.RemoveEmptyEntries);
						text = array2[0];
						num2 = int.Parse(array2[1]);
					}
					if (text.Contains("?stack="))
					{
						string[] array3 = text.Split(new string[]
						{
							"?stack="
						}, StringSplitOptions.RemoveEmptyEntries);
						text = array3[0];
						num3 = int.Parse(array3[1]);
					}
					if (child.transform.childCount > 0)
					{
						using (IEnumerator enumerator = child.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								Transform transform2 = (Transform)obj;
								GameObject gameObject2 = Network.Instantiate(this.GetPrefab(text), transform2.position, transform2.transform.rotation, default(NetworkPlayer));
								if (num >= 0 && gameObject2.GetComponent<MaterialSyncScript>())
								{
									gameObject2.GetComponent<MaterialSyncScript>().SetMaterial(num);
								}
								if (num2 >= 0 && gameObject2.GetComponent<MeshSyncScript>())
								{
									gameObject2.GetComponent<MeshSyncScript>().SetMesh(num2);
								}
								if (num3 >= 0 && gameObject2.GetComponent<StackObject>())
								{
									gameObject2.GetComponent<StackObject>().num_objects_ = num3;
								}
							}
							goto IL_2F3;
						}
					}
					GameObject prefab = this.GetPrefab(text);
					GameObject gameObject3 = Network.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, default(NetworkPlayer));
					if (num >= 0 && gameObject3.GetComponent<MaterialSyncScript>())
					{
						gameObject3.GetComponent<MaterialSyncScript>().SetMaterial(num);
					}
					if (num2 >= 0 && gameObject3.GetComponent<MeshSyncScript>())
					{
						gameObject3.GetComponent<MeshSyncScript>().SetMesh(num2);
					}
					if (num3 >= 0 && gameObject3.GetComponent<StackObject>())
					{
						gameObject3.GetComponent<StackObject>().num_objects_ = num3;
					}
				}
				IL_2F3:;
			}
			return;
		}
		Transform transform3 = gameObject.transform.Find("!Board");
		if (transform3)
		{
			for (int j = 0; j < transform3.transform.childCount; j++)
			{
				string name = transform3.transform.GetChild(j).gameObject.name;
				GameObject prefab2 = this.GetPrefab(name);
				Network.Instantiate(prefab2, prefab2.transform.position, prefab2.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform4 = gameObject.transform.Find("D2 Spawns");
		if (transform4)
		{
			foreach (object obj2 in transform4.transform)
			{
				Transform transform5 = (Transform)obj2;
				Network.Instantiate(this.D2, transform5.position, this.D2.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform6 = gameObject.transform.Find("D4 Spawns");
		if (transform6)
		{
			foreach (object obj3 in transform6.transform)
			{
				Transform transform7 = (Transform)obj3;
				Network.Instantiate(this.D4, transform7.position, Quaternion.identity, default(NetworkPlayer));
			}
		}
		Transform transform8 = gameObject.transform.Find("D4 Chrome Spawns");
		if (transform8)
		{
			foreach (object obj4 in transform8.transform)
			{
				Transform transform9 = (Transform)obj4;
				Network.Instantiate(this.D4, transform9.position, Quaternion.identity, default(NetworkPlayer)).GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
			}
		}
		Transform transform10 = gameObject.transform.Find("D6 Spawns");
		if (transform10)
		{
			foreach (object obj5 in transform10.transform)
			{
				Transform transform11 = (Transform)obj5;
				Network.Instantiate(this.D6, transform11.position, Quaternion.identity, default(NetworkPlayer));
			}
		}
		Transform transform12 = gameObject.transform.Find("D6Rounded Spawns");
		if (transform12)
		{
			foreach (object obj6 in transform12.transform)
			{
				Transform transform13 = (Transform)obj6;
				Network.Instantiate(this.D6Rounded, transform13.position, Quaternion.identity, default(NetworkPlayer));
			}
		}
		Transform transform14 = gameObject.transform.Find("D6Rounded Red Spawns");
		if (transform14)
		{
			foreach (object obj7 in transform14.transform)
			{
				Transform transform15 = (Transform)obj7;
				Network.Instantiate(this.D6Rounded, transform15.position, Quaternion.identity, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(1);
			}
		}
		Transform transform16 = gameObject.transform.Find("D6Rounded Green Spawns");
		if (transform16)
		{
			foreach (object obj8 in transform16.transform)
			{
				Transform transform17 = (Transform)obj8;
				Network.Instantiate(this.D6Rounded, transform17.position, Quaternion.identity, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(2);
			}
		}
		Transform transform18 = gameObject.transform.Find("D6Rounded Blue Spawns");
		if (transform18)
		{
			foreach (object obj9 in transform18.transform)
			{
				Transform transform19 = (Transform)obj9;
				Network.Instantiate(this.D6Rounded, transform19.position, Quaternion.identity, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(3);
			}
		}
		Transform transform20 = gameObject.transform.Find("D6 Chrome Spawns");
		if (transform20)
		{
			foreach (object obj10 in transform20.transform)
			{
				Transform transform21 = (Transform)obj10;
				Network.Instantiate(this.D6, transform21.position, Quaternion.identity, default(NetworkPlayer)).GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
			}
		}
		Transform transform22 = gameObject.transform.Find("D8 Spawns");
		if (transform22)
		{
			foreach (object obj11 in transform22.transform)
			{
				Transform transform23 = (Transform)obj11;
				Network.Instantiate(this.D8, transform23.position, Quaternion.identity, default(NetworkPlayer));
			}
		}
		Transform transform24 = gameObject.transform.Find("D8 Chrome Spawns");
		if (transform24)
		{
			foreach (object obj12 in transform24.transform)
			{
				Transform transform25 = (Transform)obj12;
				Network.Instantiate(this.D8, transform25.position, Quaternion.identity, default(NetworkPlayer)).GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
			}
		}
		Transform transform26 = gameObject.transform.Find("D10 Spawns");
		if (transform26)
		{
			foreach (object obj13 in transform26.transform)
			{
				Transform transform27 = (Transform)obj13;
				Network.Instantiate(this.D10, transform27.position, Quaternion.identity, default(NetworkPlayer));
			}
		}
		Transform transform28 = gameObject.transform.Find("D10 Chrome Spawns");
		if (transform28)
		{
			foreach (object obj14 in transform28.transform)
			{
				Transform transform29 = (Transform)obj14;
				Network.Instantiate(this.D10, transform29.position, Quaternion.identity, default(NetworkPlayer)).GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
			}
		}
		Transform transform30 = gameObject.transform.Find("D12 Spawns");
		if (transform30)
		{
			foreach (object obj15 in transform30.transform)
			{
				Transform transform31 = (Transform)obj15;
				Network.Instantiate(this.D12, transform31.position, Quaternion.identity, default(NetworkPlayer));
			}
		}
		Transform transform32 = gameObject.transform.Find("D12 Chrome Spawns");
		if (transform32)
		{
			foreach (object obj16 in transform32.transform)
			{
				Transform transform33 = (Transform)obj16;
				Network.Instantiate(this.D12, transform33.position, Quaternion.identity, default(NetworkPlayer)).GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
			}
		}
		Transform transform34 = gameObject.transform.Find("D20 Spawns");
		if (transform34)
		{
			foreach (object obj17 in transform34.transform)
			{
				Transform transform35 = (Transform)obj17;
				Network.Instantiate(this.D20, transform35.position, Quaternion.identity, default(NetworkPlayer));
			}
		}
		Transform transform36 = gameObject.transform.Find("D20 Chrome Spawns");
		if (transform36)
		{
			foreach (object obj18 in transform36.transform)
			{
				Transform transform37 = (Transform)obj18;
				Network.Instantiate(this.D20, transform37.position, Quaternion.identity, default(NetworkPlayer)).GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
			}
		}
		Transform transform38 = gameObject.transform.Find("Bag Spawns");
		if (transform38)
		{
			foreach (object obj19 in transform38.transform)
			{
				Transform transform39 = (Transform)obj19;
				Network.Instantiate(this.Bag, transform39.position, Quaternion.identity, default(NetworkPlayer));
			}
		}
		Transform transform40 = gameObject.transform.Find("Card Spawns");
		if (transform40 && transform40.childCount > 0)
		{
			this.IntList = new List<int>(52);
			for (int k = 0; k < 52; k++)
			{
				this.IntList.Add(k);
			}
			this.IntList.Randomize<int>();
			foreach (object obj20 in transform40.transform)
			{
				Transform transform41 = (Transform)obj20;
				if (this.IntList.Count > 0)
				{
					Quaternion rotation = Quaternion.identity;
					if (transform41.name == "Flip")
					{
						rotation = Quaternion.Euler(new Vector3(-180f, 0f, 0f));
					}
					else
					{
						rotation = this.Card.transform.rotation;
					}
					GameObject gameObject4 = Network.Instantiate(this.Card, transform41.position, rotation, default(NetworkPlayer));
					NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject4, this.IntList[0], -1, false);
					gameObject4.GetComponent<CardScript>().card_id_ = this.IntList[0];
					this.IntList.RemoveAt(0);
				}
				else
				{
					Debug.Log("Trying to spawn too many");
				}
			}
		}
		Transform transform42 = gameObject.transform.Find("Deck Spawns");
		if (transform42)
		{
			foreach (object obj21 in transform42.transform)
			{
				Transform transform43 = (Transform)obj21;
				GameObject gameObject5 = Network.Instantiate(this.Deck, transform43.position, this.Deck.transform.rotation, default(NetworkPlayer));
				if (this.IntList.Count > 0)
				{
					DeckScript component = gameObject5.GetComponent<DeckScript>();
					component.bRandomSpawn = false;
					component.SetDeck(this.IntList, null);
					this.IntList = new List<int>();
				}
			}
		}
		Transform transform44 = gameObject.transform.Find("DeckCustom Spawns");
		if (transform44)
		{
			foreach (object obj22 in transform44.transform)
			{
				Transform transform45 = (Transform)obj22;
				Network.Instantiate(this.DeckCustom, transform45.position, this.DeckCustom.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform46 = gameObject.transform.Find("DeckCardBotMain Spawns");
		if (transform46)
		{
			foreach (object obj23 in transform46.transform)
			{
				Transform transform47 = (Transform)obj23;
				Network.Instantiate(this.DeckCardBotMain, transform47.position, this.DeckCardBotMain.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform48 = gameObject.transform.Find("DeckCardBotHead Spawns");
		if (transform48)
		{
			foreach (object obj24 in transform48.transform)
			{
				Transform transform49 = (Transform)obj24;
				Network.Instantiate(this.DeckCardBotHead, transform49.position, this.DeckCardBotHead.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform50 = gameObject.transform.Find("Poker10 Spawns");
		if (transform50)
		{
			foreach (object obj25 in transform50.transform)
			{
				Transform transform51 = (Transform)obj25;
				Network.Instantiate(this.Poker10, transform51.position, this.Poker10.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform52 = gameObject.transform.Find("Poker50 Spawns");
		if (transform52)
		{
			foreach (object obj26 in transform52.transform)
			{
				Transform transform53 = (Transform)obj26;
				Network.Instantiate(this.Poker50, transform53.position, this.Poker50.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform54 = gameObject.transform.Find("Poker100 Spawns");
		if (transform54)
		{
			foreach (object obj27 in transform54.transform)
			{
				Transform transform55 = (Transform)obj27;
				Network.Instantiate(this.Poker100, transform55.position, this.Poker100.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform56 = gameObject.transform.Find("Poker500 Spawns");
		if (transform56)
		{
			foreach (object obj28 in transform56.transform)
			{
				Transform transform57 = (Transform)obj28;
				Network.Instantiate(this.Poker500, transform57.position, this.Poker500.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform58 = gameObject.transform.Find("Poker1000 Spawns");
		if (transform58)
		{
			foreach (object obj29 in transform58.transform)
			{
				Transform transform59 = (Transform)obj29;
				Network.Instantiate(this.Poker1000, transform59.position, this.Poker1000.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform60 = gameObject.transform.Find("ChessPawnWhite Spawns");
		if (transform60)
		{
			foreach (object obj30 in transform60.transform)
			{
				Transform transform61 = (Transform)obj30;
				Network.Instantiate(this.ChessPawn, transform61.position, this.ChessPawn.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(0);
			}
		}
		Transform transform62 = gameObject.transform.Find("ChessPawnBlack Spawns");
		if (transform62)
		{
			foreach (object obj31 in transform62.transform)
			{
				Transform transform63 = (Transform)obj31;
				Network.Instantiate(this.ChessPawn, transform63.position, this.ChessPawn.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(1);
			}
		}
		Transform transform64 = gameObject.transform.Find("ChessRookWhite Spawns");
		if (transform64)
		{
			foreach (object obj32 in transform64.transform)
			{
				Transform transform65 = (Transform)obj32;
				Network.Instantiate(this.ChessRook, transform65.position, this.ChessRook.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(0);
			}
		}
		Transform transform66 = gameObject.transform.Find("ChessRookBlack Spawns");
		if (transform66)
		{
			foreach (object obj33 in transform66.transform)
			{
				Transform transform67 = (Transform)obj33;
				Network.Instantiate(this.ChessRook, transform67.position, this.ChessRook.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(1);
			}
		}
		Transform transform68 = gameObject.transform.Find("ChessKnightWhite Spawns");
		if (transform68)
		{
			foreach (object obj34 in transform68.transform)
			{
				Transform transform69 = (Transform)obj34;
				Network.Instantiate(this.ChessKnight, transform69.position, this.YAXISFLIP, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(0);
			}
		}
		Transform transform70 = gameObject.transform.Find("ChessKnightBlack Spawns");
		if (transform70)
		{
			foreach (object obj35 in transform70.transform)
			{
				Transform transform71 = (Transform)obj35;
				Network.Instantiate(this.ChessKnight, transform71.position, this.ChessKnight.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(1);
			}
		}
		Transform transform72 = gameObject.transform.Find("ChessBishopWhite Spawns");
		if (transform72)
		{
			foreach (object obj36 in transform72.transform)
			{
				Transform transform73 = (Transform)obj36;
				Network.Instantiate(this.ChessBishop, transform73.position, this.YAXISFLIP, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(0);
			}
		}
		Transform transform74 = gameObject.transform.Find("ChessBishopBlack Spawns");
		if (transform74)
		{
			foreach (object obj37 in transform74.transform)
			{
				Transform transform75 = (Transform)obj37;
				Network.Instantiate(this.ChessBishop, transform75.position, this.ChessBishop.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(1);
			}
		}
		Transform transform76 = gameObject.transform.Find("ChessQueenWhite Spawns");
		if (transform76)
		{
			foreach (object obj38 in transform76.transform)
			{
				Transform transform77 = (Transform)obj38;
				Network.Instantiate(this.ChessQueen, transform77.position, this.ChessQueen.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(0);
			}
		}
		Transform transform78 = gameObject.transform.Find("ChessQueenBlack Spawns");
		if (transform78)
		{
			foreach (object obj39 in transform78.transform)
			{
				Transform transform79 = (Transform)obj39;
				Network.Instantiate(this.ChessQueen, transform79.position, this.ChessQueen.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(1);
			}
		}
		Transform transform80 = gameObject.transform.Find("ChessKingWhite Spawns");
		if (transform80)
		{
			foreach (object obj40 in transform80.transform)
			{
				Transform transform81 = (Transform)obj40;
				Network.Instantiate(this.ChessKing, transform81.position, this.ChessKing.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(0);
			}
		}
		Transform transform82 = gameObject.transform.Find("ChessKingBlack Spawns");
		if (transform82)
		{
			foreach (object obj41 in transform82.transform)
			{
				Transform transform83 = (Transform)obj41;
				Network.Instantiate(this.ChessKing, transform83.position, this.ChessKing.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(1);
			}
		}
		Transform transform84 = gameObject.transform.Find("CheckerWhite Spawns");
		if (transform84)
		{
			foreach (object obj42 in transform84.transform)
			{
				Transform transform85 = (Transform)obj42;
				Network.Instantiate(this.CheckerWhite, transform85.position, this.CheckerWhite.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform86 = gameObject.transform.Find("CheckerBlack Spawns");
		if (transform86)
		{
			foreach (object obj43 in transform86.transform)
			{
				Transform transform87 = (Transform)obj43;
				Network.Instantiate(this.CheckerBlack, transform87.position, this.CheckerBlack.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform88 = gameObject.transform.Find("CheckerRed Spawns");
		if (transform88)
		{
			foreach (object obj44 in transform88.transform)
			{
				Transform transform89 = (Transform)obj44;
				Network.Instantiate(this.CheckerRed, transform89.position, this.CheckerRed.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform90 = gameObject.transform.Find("Domino Spawns");
		if (transform90)
		{
			this.IntList = new List<int>(this.NumDominoes);
			for (int l = 0; l < this.NumDominoes; l++)
			{
				this.IntList.Add(l);
			}
			this.IntList.Randomize<int>();
			foreach (object obj45 in transform90.transform)
			{
				Transform transform91 = (Transform)obj45;
				if (this.IntList.Count > 0)
				{
					Network.Instantiate(this.Domino, transform91.position, this.Domino.transform.rotation, default(NetworkPlayer)).GetComponent<MeshSyncScript>().SetMesh(this.IntList[0]);
					this.IntList.RemoveAt(0);
				}
				else
				{
					Debug.Log("Trying to spawn too many Dominoes");
				}
			}
		}
		Transform transform92 = gameObject.transform.Find("ReversiChipWhite Spawns");
		if (transform92)
		{
			foreach (object obj46 in transform92.transform)
			{
				Transform transform93 = (Transform)obj46;
				Network.Instantiate(this.ReversiChip, transform93.position, this.ReversiChip.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform94 = gameObject.transform.Find("ReversiChipBlack Spawns");
		if (transform94)
		{
			foreach (object obj47 in transform94.transform)
			{
				Transform transform95 = (Transform)obj47;
				Quaternion rotation2 = Quaternion.Euler(new Vector3(-180f, 0f, 0f));
				Network.Instantiate(this.ReversiChip, transform95.position, rotation2, default(NetworkPlayer));
			}
		}
		Transform transform96 = gameObject.transform.Find("BackgammonPieceWhite Spawns");
		if (transform96)
		{
			foreach (object obj48 in transform96.transform)
			{
				Transform transform97 = (Transform)obj48;
				Network.Instantiate(this.BackgammonPieceWhite, transform97.position, this.BackgammonPieceWhite.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform98 = gameObject.transform.Find("BackgammonPieceBrown Spawns");
		if (transform98)
		{
			foreach (object obj49 in transform98.transform)
			{
				Transform transform99 = (Transform)obj49;
				Network.Instantiate(this.BackgammonPieceBrown, transform99.position, this.BackgammonPieceBrown.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform100 = gameObject.transform.Find("ChineseCheckersWhite Spawns");
		if (transform100)
		{
			foreach (object obj50 in transform100.transform)
			{
				Transform transform101 = (Transform)obj50;
				Network.Instantiate(this.ChineseCheckersPiece, transform101.position, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform102 = gameObject.transform.Find("ChineseCheckersRed Spawns");
		if (transform102)
		{
			foreach (object obj51 in transform102.transform)
			{
				Transform transform103 = (Transform)obj51;
				Network.Instantiate(this.ChineseCheckersPiece, transform103.position, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(1);
			}
		}
		Transform transform104 = gameObject.transform.Find("ChineseCheckersYellow Spawns");
		if (transform104)
		{
			foreach (object obj52 in transform104.transform)
			{
				Transform transform105 = (Transform)obj52;
				Network.Instantiate(this.ChineseCheckersPiece, transform105.position, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(2);
			}
		}
		Transform transform106 = gameObject.transform.Find("ChineseCheckersGreen Spawns");
		if (transform106)
		{
			foreach (object obj53 in transform106.transform)
			{
				Transform transform107 = (Transform)obj53;
				Network.Instantiate(this.ChineseCheckersPiece, transform107.position, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(3);
			}
		}
		Transform transform108 = gameObject.transform.Find("ChineseCheckersBlue Spawns");
		if (transform108)
		{
			foreach (object obj54 in transform108.transform)
			{
				Transform transform109 = (Transform)obj54;
				Network.Instantiate(this.ChineseCheckersPiece, transform109.position, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(4);
			}
		}
		Transform transform110 = gameObject.transform.Find("ChineseCheckersPink Spawns");
		if (transform110)
		{
			foreach (object obj55 in transform110.transform)
			{
				Transform transform111 = (Transform)obj55;
				Network.Instantiate(this.ChineseCheckersPiece, transform111.position, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(5);
			}
		}
		Transform transform112 = gameObject.transform.Find("ChineseCheckersBlack Spawns");
		if (transform112)
		{
			foreach (object obj56 in transform112.transform)
			{
				Transform transform113 = (Transform)obj56;
				Network.Instantiate(this.ChineseCheckersPiece, transform113.position, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(6);
			}
		}
		Transform transform114 = gameObject.transform.Find("GoPieceWhite Spawns");
		if (transform114)
		{
			foreach (object obj57 in transform114.transform)
			{
				Transform transform115 = (Transform)obj57;
				Network.Instantiate(this.GoPieceWhite, transform115.position, this.GoPieceWhite.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform116 = gameObject.transform.Find("GoPieceBlack Spawns");
		if (transform116)
		{
			foreach (object obj58 in transform116.transform)
			{
				Transform transform117 = (Transform)obj58;
				Network.Instantiate(this.GoPieceBlack, transform117.position, this.GoPieceBlack.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform118 = gameObject.transform.Find("GoBowlWhite Spawns");
		if (transform118)
		{
			foreach (object obj59 in transform118.transform)
			{
				Transform transform119 = (Transform)obj59;
				Network.Instantiate(this.GoBowlWhite, transform119.position, this.GoBowlWhite.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform120 = gameObject.transform.Find("GoBowlBlack Spawns");
		if (transform120)
		{
			foreach (object obj60 in transform120.transform)
			{
				Transform transform121 = (Transform)obj60;
				Network.Instantiate(this.GoBowlBlack, transform121.position, this.GoBowlBlack.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform122 = gameObject.transform.Find("BlockSquare Spawns");
		if (transform122)
		{
			foreach (object obj61 in transform122.transform)
			{
				Transform transform123 = (Transform)obj61;
				Network.Instantiate(this.BlockSquare, transform123.position, this.BlockSquare.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform124 = gameObject.transform.Find("BlockRectangle Spawns");
		if (transform124)
		{
			foreach (object obj62 in transform124.transform)
			{
				Transform transform125 = (Transform)obj62;
				Network.Instantiate(this.BlockRectangle, transform125.position, this.BlockRectangle.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform126 = gameObject.transform.Find("BlockTriangle Spawns");
		if (transform126)
		{
			foreach (object obj63 in transform126.transform)
			{
				Transform transform127 = (Transform)obj63;
				Network.Instantiate(this.BlockTriangle, transform127.position, this.BlockTriangle.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform128 = gameObject.transform.Find("PlayerPawnWhite Spawns");
		if (transform128)
		{
			foreach (object obj64 in transform128.transform)
			{
				Transform transform129 = (Transform)obj64;
				Network.Instantiate(this.PlayerPawn, transform129.position, this.PlayerPawn.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform130 = gameObject.transform.Find("PlayerPawnRed Spawns");
		if (transform130)
		{
			foreach (object obj65 in transform130.transform)
			{
				Transform transform131 = (Transform)obj65;
				Network.Instantiate(this.PlayerPawn, transform131.position, this.PlayerPawn.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(1);
			}
		}
		Transform transform132 = gameObject.transform.Find("PlayerPawnOrange Spawns");
		if (transform132)
		{
			foreach (object obj66 in transform132.transform)
			{
				Transform transform133 = (Transform)obj66;
				Network.Instantiate(this.PlayerPawn, transform133.position, this.PlayerPawn.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(2);
			}
		}
		Transform transform134 = gameObject.transform.Find("PlayerPawnYellow Spawns");
		if (transform134)
		{
			foreach (object obj67 in transform134.transform)
			{
				Transform transform135 = (Transform)obj67;
				Network.Instantiate(this.PlayerPawn, transform135.position, this.PlayerPawn.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(3);
			}
		}
		Transform transform136 = gameObject.transform.Find("PlayerPawnGreen Spawns");
		if (transform136)
		{
			foreach (object obj68 in transform136.transform)
			{
				Transform transform137 = (Transform)obj68;
				Network.Instantiate(this.PlayerPawn, transform137.position, this.PlayerPawn.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(4);
			}
		}
		Transform transform138 = gameObject.transform.Find("PlayerPawnBlue Spawns");
		if (transform138)
		{
			foreach (object obj69 in transform138.transform)
			{
				Transform transform139 = (Transform)obj69;
				Network.Instantiate(this.PlayerPawn, transform139.position, this.PlayerPawn.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(5);
			}
		}
		Transform transform140 = gameObject.transform.Find("PlayerPawnPurple Spawns");
		if (transform140)
		{
			foreach (object obj70 in transform140.transform)
			{
				Transform transform141 = (Transform)obj70;
				Network.Instantiate(this.PlayerPawn, transform141.position, this.PlayerPawn.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(6);
			}
		}
		Transform transform142 = gameObject.transform.Find("PlayerPawnPink Spawns");
		if (transform142)
		{
			foreach (object obj71 in transform142.transform)
			{
				Transform transform143 = (Transform)obj71;
				Network.Instantiate(this.PlayerPawn, transform143.position, this.PlayerPawn.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(7);
			}
		}
		Transform transform144 = gameObject.transform.Find("PlayerPawnBlack Spawns");
		if (transform144)
		{
			foreach (object obj72 in transform144.transform)
			{
				Transform transform145 = (Transform)obj72;
				Network.Instantiate(this.PlayerPawn, transform145.position, this.PlayerPawn.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(8);
			}
		}
		Transform transform146 = gameObject.transform.Find("DiePiecepackArms Spawns");
		if (transform146)
		{
			foreach (object obj73 in transform146.transform)
			{
				Transform transform147 = (Transform)obj73;
				Network.Instantiate(this.DiePiecepack, transform147.position, this.DiePiecepack.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(0);
			}
		}
		Transform transform148 = gameObject.transform.Find("DiePiecepackCrowns Spawns");
		if (transform148)
		{
			foreach (object obj74 in transform148.transform)
			{
				Transform transform149 = (Transform)obj74;
				Network.Instantiate(this.DiePiecepack, transform149.position, this.DiePiecepack.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(1);
			}
		}
		Transform transform150 = gameObject.transform.Find("DiePiecepackMoons Spawns");
		if (transform150)
		{
			foreach (object obj75 in transform150.transform)
			{
				Transform transform151 = (Transform)obj75;
				Network.Instantiate(this.DiePiecepack, transform151.position, this.DiePiecepack.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(2);
			}
		}
		Transform transform152 = gameObject.transform.Find("DiePiecepackSuns Spawns");
		if (transform152)
		{
			foreach (object obj76 in transform152.transform)
			{
				Transform transform153 = (Transform)obj76;
				Network.Instantiate(this.DiePiecepack, transform153.position, this.DiePiecepack.transform.rotation, default(NetworkPlayer)).GetComponent<MaterialSyncScript>().SetMaterial(3);
			}
		}
		for (int m = 0; m < 12; m++)
		{
			Transform transform154 = gameObject.transform.Find("PiecepackSuns" + m + " Spawns");
			if (transform154)
			{
				foreach (object obj77 in transform154.transform)
				{
					Transform transform155 = (Transform)obj77;
					Network.Instantiate(this.PiecepackSuns, transform155.position, this.PiecepackSuns.transform.rotation, default(NetworkPlayer)).GetComponent<MeshSyncScript>().SetMesh(m);
				}
			}
		}
		for (int n = 0; n < 12; n++)
		{
			Transform transform156 = gameObject.transform.Find("PiecepackCrowns" + n + " Spawns");
			if (transform156)
			{
				foreach (object obj78 in transform156.transform)
				{
					Transform transform157 = (Transform)obj78;
					Network.Instantiate(this.PiecepackCrowns, transform157.position, this.PiecepackCrowns.transform.rotation, default(NetworkPlayer)).GetComponent<MeshSyncScript>().SetMesh(n);
				}
			}
		}
		for (int num4 = 0; num4 < 12; num4++)
		{
			Transform transform158 = gameObject.transform.Find("PiecepackMoons" + num4 + " Spawns");
			if (transform158)
			{
				foreach (object obj79 in transform158.transform)
				{
					Transform transform159 = (Transform)obj79;
					Network.Instantiate(this.PiecepackMoons, transform159.position, this.PiecepackMoons.transform.rotation, default(NetworkPlayer)).GetComponent<MeshSyncScript>().SetMesh(num4);
				}
			}
		}
		for (int num5 = 0; num5 < 12; num5++)
		{
			Transform transform160 = gameObject.transform.Find("PiecepackArms" + num5 + " Spawns");
			if (transform160)
			{
				foreach (object obj80 in transform160.transform)
				{
					Transform transform161 = (Transform)obj80;
					Network.Instantiate(this.PiecepackArms, transform161.position, this.PiecepackArms.transform.rotation, default(NetworkPlayer)).GetComponent<MeshSyncScript>().SetMesh(num5);
				}
			}
		}
		int num6 = 4;
		for (int num7 = 0; num7 < num6; num7++)
		{
			Transform transform162 = gameObject.transform.Find("MahjongCoin" + num7.ToString() + " Spawns");
			if (transform162)
			{
				foreach (object obj81 in transform162.transform)
				{
					Transform transform163 = (Transform)obj81;
					Network.Instantiate(this.MahjongCoin, transform163.position, this.MahjongCoin.transform.rotation, default(NetworkPlayer)).GetComponent<MeshSyncScript>().SetMesh(num7);
				}
			}
		}
		int num8 = 4;
		for (int num9 = 0; num9 < num8; num9++)
		{
			Transform transform164 = gameObject.transform.Find("MahjongStick" + num9.ToString() + " Spawns");
			if (transform164)
			{
				foreach (object obj82 in transform164.transform)
				{
					Transform transform165 = (Transform)obj82;
					Network.Instantiate(this.MahjongStick, transform165.position, this.MahjongStick.transform.rotation, default(NetworkPlayer)).GetComponent<MeshSyncScript>().SetMesh(num9);
				}
			}
		}
		int num10 = 36;
		Transform transform166 = gameObject.transform.Find("MahjongTile Spawns");
		if (transform166)
		{
			this.IntList = new List<int>(num10);
			for (int num11 = 0; num11 < num10; num11++)
			{
				this.IntList.Add(num11);
				this.IntList.Add(num11);
				this.IntList.Add(num11);
				this.IntList.Add(num11);
			}
			this.IntList.Randomize<int>();
			foreach (object obj83 in transform166.transform)
			{
				Transform transform167 = (Transform)obj83;
				if (this.IntList.Count > 0)
				{
					Network.Instantiate(this.MahjongTile, transform167.position, transform167.rotation, default(NetworkPlayer)).GetComponent<MeshSyncScript>().SetMesh(this.IntList[0]);
					this.IntList.RemoveAt(0);
				}
				else
				{
					Debug.Log("Trying to spawn too many MahjongTiles");
				}
			}
		}
		Transform transform168 = gameObject.transform.Find("rpgORC Spawns");
		if (transform168)
		{
			foreach (object obj84 in transform168.transform)
			{
				Transform transform169 = (Transform)obj84;
				Network.Instantiate(this.rpgORC, transform169.position, this.rpgORC.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform170 = gameObject.transform.Find("rpgBEAR Spawns");
		if (transform170)
		{
			foreach (object obj85 in transform170.transform)
			{
				Transform transform171 = (Transform)obj85;
				Network.Instantiate(this.rpgBEAR, transform171.position, this.rpgBEAR.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform172 = gameObject.transform.Find("rpgTROLL Spawns");
		if (transform172)
		{
			foreach (object obj86 in transform172.transform)
			{
				Transform transform173 = (Transform)obj86;
				Network.Instantiate(this.rpgTROLL, transform173.position, this.rpgTROLL.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform174 = gameObject.transform.Find("rpgEVILWATCHER Spawns");
		if (transform174)
		{
			foreach (object obj87 in transform174.transform)
			{
				Transform transform175 = (Transform)obj87;
				Network.Instantiate(this.rpgEVILWATCHER, transform175.position, this.rpgEVILWATCHER.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform176 = gameObject.transform.Find("rpgGIANTVIPER Spawns");
		if (transform176)
		{
			foreach (object obj88 in transform176.transform)
			{
				Transform transform177 = (Transform)obj88;
				Network.Instantiate(this.rpgGIANTVIPER, transform177.position, this.rpgGIANTVIPER.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform178 = gameObject.transform.Find("rpgGOBLIN Spawns");
		if (transform178)
		{
			foreach (object obj89 in transform178.transform)
			{
				Transform transform179 = (Transform)obj89;
				Network.Instantiate(this.rpgGOBLIN, transform179.position, this.rpgGOBLIN.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform180 = gameObject.transform.Find("rpgGOLEM Spawns");
		if (transform180)
		{
			foreach (object obj90 in transform180.transform)
			{
				Transform transform181 = (Transform)obj90;
				Network.Instantiate(this.rpgGOLEM, transform181.position, this.rpgGOLEM.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform182 = gameObject.transform.Find("rpgGRIFFON Spawns");
		if (transform182)
		{
			foreach (object obj91 in transform182.transform)
			{
				Transform transform183 = (Transform)obj91;
				Network.Instantiate(this.rpgGRIFFON, transform183.position, this.rpgGRIFFON.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform184 = gameObject.transform.Find("rpgHYDRA Spawns");
		if (transform184)
		{
			foreach (object obj92 in transform184.transform)
			{
				Transform transform185 = (Transform)obj92;
				Network.Instantiate(this.rpgHYDRA, transform185.position, this.rpgHYDRA.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform186 = gameObject.transform.Find("rpgMUMMY Spawns");
		if (transform186)
		{
			foreach (object obj93 in transform186.transform)
			{
				Transform transform187 = (Transform)obj93;
				Network.Instantiate(this.rpgMUMMY, transform187.position, this.rpgMUMMY.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform188 = gameObject.transform.Find("rpgVAMPIRE Spawns");
		if (transform188)
		{
			foreach (object obj94 in transform188.transform)
			{
				Transform transform189 = (Transform)obj94;
				Network.Instantiate(this.rpgVAMPIRE, transform189.position, this.rpgVAMPIRE.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform190 = gameObject.transform.Find("rpgRAT Spawns");
		if (transform190)
		{
			foreach (object obj95 in transform190.transform)
			{
				Transform transform191 = (Transform)obj95;
				Network.Instantiate(this.rpgRAT, transform191.position, this.rpgRAT.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform192 = gameObject.transform.Find("rpgTREEENT Spawns");
		if (transform192)
		{
			foreach (object obj96 in transform192.transform)
			{
				Transform transform193 = (Transform)obj96;
				Network.Instantiate(this.rpgTREEENT, transform193.position, this.rpgTREEENT.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform194 = gameObject.transform.Find("rpgWOLF Spawns");
		if (transform194)
		{
			foreach (object obj97 in transform194.transform)
			{
				Transform transform195 = (Transform)obj97;
				Network.Instantiate(this.rpgWOLF, transform195.position, this.rpgWOLF.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform196 = gameObject.transform.Find("rpgWEREWOLF Spawns");
		if (transform196)
		{
			foreach (object obj98 in transform196.transform)
			{
				Transform transform197 = (Transform)obj98;
				Network.Instantiate(this.rpgWEREWOLF, transform197.position, this.rpgWEREWOLF.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform198 = gameObject.transform.Find("rpgCHIMERA Spawns");
		if (transform198)
		{
			foreach (object obj99 in transform198.transform)
			{
				Transform transform199 = (Transform)obj99;
				Network.Instantiate(this.rpgCHIMERA, transform199.position, this.rpgCHIMERA.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform200 = gameObject.transform.Find("rpgLIZARDWARRIOR Spawns");
		if (transform200)
		{
			foreach (object obj100 in transform200.transform)
			{
				Transform transform201 = (Transform)obj100;
				Network.Instantiate(this.rpgLIZARDWARRIOR, transform201.position, this.rpgLIZARDWARRIOR.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform202 = gameObject.transform.Find("rpgDRAGONIDE Spawns");
		if (transform202)
		{
			foreach (object obj101 in transform202.transform)
			{
				Transform transform203 = (Transform)obj101;
				Network.Instantiate(this.rpgDRAGONIDE, transform203.position, this.rpgDRAGONIDE.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform204 = gameObject.transform.Find("rpgOGRE Spawns");
		if (transform204)
		{
			foreach (object obj102 in transform204.transform)
			{
				Transform transform205 = (Transform)obj102;
				Network.Instantiate(this.rpgOGRE, transform205.position, this.rpgOGRE.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform206 = gameObject.transform.Find("rpgGHOUL Spawns");
		if (transform206)
		{
			foreach (object obj103 in transform206.transform)
			{
				Transform transform207 = (Transform)obj103;
				Network.Instantiate(this.rpgGHOUL, transform207.position, this.rpgGHOUL.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform208 = gameObject.transform.Find("rpgWYVERN Spawns");
		if (transform208)
		{
			foreach (object obj104 in transform208.transform)
			{
				Transform transform209 = (Transform)obj104;
				Network.Instantiate(this.rpgWYVERN, transform209.position, this.rpgWYVERN.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform210 = gameObject.transform.Find("rpgCYCLOPS Spawns");
		if (transform210)
		{
			foreach (object obj105 in transform210.transform)
			{
				Transform transform211 = (Transform)obj105;
				Network.Instantiate(this.rpgCYCLOPS, transform211.position, this.rpgCYCLOPS.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform212 = gameObject.transform.Find("rpgKOBOLD Spawns");
		if (transform212)
		{
			foreach (object obj106 in transform212.transform)
			{
				Transform transform213 = (Transform)obj106;
				Network.Instantiate(this.rpgKOBOLD, transform213.position, this.rpgKOBOLD.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform214 = gameObject.transform.Find("rpgSKELETONKNIGHT Spawns");
		if (transform214)
		{
			foreach (object obj107 in transform214.transform)
			{
				Transform transform215 = (Transform)obj107;
				Network.Instantiate(this.rpgSKELETONKNIGHT, transform215.position, this.rpgSKELETONKNIGHT.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform216 = gameObject.transform.Find("rpgMANTICORA Spawns");
		if (transform216)
		{
			foreach (object obj108 in transform216.transform)
			{
				Transform transform217 = (Transform)obj108;
				Network.Instantiate(this.rpgMANTICORA, transform217.position, this.rpgMANTICORA.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform218 = gameObject.transform.Find("TilesetFloor Spawns");
		if (transform218)
		{
			foreach (object obj109 in transform218.transform)
			{
				Transform transform219 = (Transform)obj109;
				Network.Instantiate(this.TilesetFloor, transform219.position, this.TilesetFloor.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform220 = gameObject.transform.Find("TilesetWall Spawns");
		if (transform220)
		{
			foreach (object obj110 in transform220.transform)
			{
				Transform transform221 = (Transform)obj110;
				Network.Instantiate(this.TilesetWall, transform221.position, this.TilesetWall.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform222 = gameObject.transform.Find("TilesetCorner Spawns");
		if (transform222)
		{
			foreach (object obj111 in transform222.transform)
			{
				Transform transform223 = (Transform)obj111;
				Network.Instantiate(this.TilesetCorner, transform223.position, this.TilesetCorner.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform224 = gameObject.transform.Find("TilesetChair Spawns");
		if (transform224)
		{
			foreach (object obj112 in transform224.transform)
			{
				Transform transform225 = (Transform)obj112;
				Network.Instantiate(this.TilesetChair, transform225.position, this.TilesetChair.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform226 = gameObject.transform.Find("TilesetTable Spawns");
		if (transform226)
		{
			foreach (object obj113 in transform226.transform)
			{
				Transform transform227 = (Transform)obj113;
				Network.Instantiate(this.TilesetTable, transform227.position, this.TilesetTable.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform228 = gameObject.transform.Find("TilesetBarrel Spawns");
		if (transform228)
		{
			foreach (object obj114 in transform228.transform)
			{
				Transform transform229 = (Transform)obj114;
				Network.Instantiate(this.TilesetBarrel, transform229.position, this.TilesetBarrel.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform230 = gameObject.transform.Find("TilesetChest Spawns");
		if (transform230)
		{
			foreach (object obj115 in transform230.transform)
			{
				Transform transform231 = (Transform)obj115;
				Network.Instantiate(this.TilesetChest, transform231.position, this.TilesetChest.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform232 = gameObject.transform.Find("Tablet Spawns");
		if (transform232)
		{
			foreach (object obj116 in transform232.transform)
			{
				Transform transform233 = (Transform)obj116;
				Network.Instantiate(this.Tablet, transform233.position, this.Tablet.transform.rotation, default(NetworkPlayer));
			}
		}
		Transform transform234 = gameObject.transform.Find("Test Spawns");
		if (transform234)
		{
			foreach (object obj117 in transform234.transform)
			{
				Transform transform235 = (Transform)obj117;
				Network.Instantiate(this.Test, transform235.position, this.Test.transform.rotation, default(NetworkPlayer));
			}
		}
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x00061C7C File Offset: 0x0005FE7C
	public GameObject SpawnNameCoroutineFromUI(string ObjectName, Vector3 SpawnPos, bool bLocalSpawn = false, bool bSnapToGrid = true, bool bSpawnSound = true)
	{
		return this.SpawnNameCoroutine(ObjectName, SpawnPos, bLocalSpawn, bSnapToGrid, bSpawnSound, true);
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x00061C8C File Offset: 0x0005FE8C
	public GameObject SpawnNameCoroutine(string ObjectName, Vector3 SpawnPos, bool bLocalSpawn = false, bool bSnapToGrid = true, bool bSpawnSound = true, bool spawnedFromUI = false)
	{
		GameObject gameObject = this.SpawnName(ObjectName, SpawnPos, bLocalSpawn);
		if (!bLocalSpawn && gameObject && Network.isServer)
		{
			if (bSnapToGrid)
			{
				base.StartCoroutine(NetworkSingleton<ManagerPhysicsObject>.Instance.DelaySnapToGrid(gameObject));
			}
			if (bSpawnSound && gameObject.GetComponent<SoundScript>())
			{
				gameObject.GetComponent<SoundScript>().CopyPasteSound();
			}
			if (spawnedFromUI)
			{
				gameObject.GetComponent<NetworkPhysicsObject>().spawnedByUI = true;
			}
		}
		return gameObject;
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x00061CF8 File Offset: 0x0005FEF8
	[Remote(Permission.Admin)]
	public void SpawnNameRPC(string ObjectName, Vector3 SpawnPos)
	{
		this.SpawnNameCoroutine(ObjectName, SpawnPos, false, true, true, false);
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x00061D08 File Offset: 0x0005FF08
	public GameObject SpawnName(string ObjectName, Vector3 SpawnPos, bool bLocalSpawn = false)
	{
		if (!ObjectName.Contains("Dice"))
		{
			if (ObjectName.StartsWith("Arms "))
			{
				ObjectName = ObjectName.Replace("Arms ", "");
				this.Piecepackpiece = NetworkSingleton<GameMode>.Instance.PiecepackArms;
			}
			if (ObjectName.StartsWith("Crowns "))
			{
				ObjectName = ObjectName.Replace("Crowns ", "");
				this.Piecepackpiece = NetworkSingleton<GameMode>.Instance.PiecepackCrowns;
			}
			if (ObjectName.StartsWith("Moons "))
			{
				ObjectName = ObjectName.Replace("Moons ", "");
				this.Piecepackpiece = NetworkSingleton<GameMode>.Instance.PiecepackMoons;
			}
			if (ObjectName.StartsWith("Suns "))
			{
				ObjectName = ObjectName.Replace("Suns ", "");
				this.Piecepackpiece = NetworkSingleton<GameMode>.Instance.PiecepackSuns;
			}
		}
		if (!bLocalSpawn)
		{
			if (Network.isClient)
			{
				base.networkView.RPC<string, Vector3>(RPCTarget.Server, new Action<string, Vector3>(this.SpawnNameRPC), ObjectName, SpawnPos);
				return null;
			}
			if (ObjectName == "Tablet")
			{
				return Network.Instantiate(this.Tablet, SpawnPos, this.Tablet.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Digital Clock")
			{
				return Network.Instantiate(this.DigitalClock, SpawnPos, this.DigitalClock.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Red Square")
			{
				return Network.Instantiate(this.BlockSquare, SpawnPos, this.BlockSquare.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Blue Rectangle")
			{
				return Network.Instantiate(this.BlockRectangle, SpawnPos, this.BlockRectangle.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Green Triangle")
			{
				return Network.Instantiate(this.BlockTriangle, SpawnPos, this.BlockTriangle.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Standard Deck")
			{
				return Network.Instantiate(this.Deck, SpawnPos, this.Deck.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Custom Deck")
			{
				return Network.Instantiate(this.DeckCustom, SpawnPos, this.Deck.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Random Card")
			{
				GameObject gameObject = Network.Instantiate(this.Card, SpawnPos, this.Card.transform.rotation, default(NetworkPlayer));
				int num = UnityEngine.Random.Range(0, 51);
				NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject, num, -1, false);
				gameObject.GetComponent<CardScript>().card_id_ = num;
				return gameObject;
			}
			if (ObjectName == "Joker")
			{
				GameObject gameObject2 = Network.Instantiate(this.Card, SpawnPos, this.Card.transform.rotation, default(NetworkPlayer));
				NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject2, 52, -1, false);
				gameObject2.GetComponent<CardScript>().card_id_ = 52;
				return gameObject2;
			}
			if (ObjectName == "CardBots Main Deck")
			{
				return Network.Instantiate(this.DeckCardBotMain, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "CardBots Head Deck")
			{
				return Network.Instantiate(this.DeckCardBotHead, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Red Checker")
			{
				return Network.Instantiate(this.CheckerRed, SpawnPos, this.CheckerRed.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Black Checker")
			{
				return Network.Instantiate(this.CheckerBlack, SpawnPos, this.CheckerBlack.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "White Checker")
			{
				return Network.Instantiate(this.CheckerWhite, SpawnPos, this.CheckerWhite.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Pawn Chrome")
			{
				return Network.Instantiate(this.ChessPawn, SpawnPos, this.ChessPawn.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Rook Chrome")
			{
				return Network.Instantiate(this.ChessRook, SpawnPos, this.ChessRook.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Knight Chrome")
			{
				return Network.Instantiate(this.ChessKnight, SpawnPos, this.ChessKnight.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Bishop Chrome")
			{
				return Network.Instantiate(this.ChessBishop, SpawnPos, this.ChessBishop.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Queen Chrome")
			{
				return Network.Instantiate(this.ChessQueen, SpawnPos, this.ChessQueen.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "King Chrome")
			{
				return Network.Instantiate(this.ChessKing, SpawnPos, this.ChessKing.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Pawn Cast Iron")
			{
				GameObject gameObject3 = Network.Instantiate(this.ChessPawn, SpawnPos, this.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject3.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject3;
			}
			if (ObjectName == "Rook Cast Iron")
			{
				GameObject gameObject4 = Network.Instantiate(this.ChessRook, SpawnPos, this.ChessRook.transform.rotation, default(NetworkPlayer));
				gameObject4.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject4;
			}
			if (ObjectName == "Knight Cast Iron")
			{
				GameObject gameObject5 = Network.Instantiate(this.ChessKnight, SpawnPos, this.ChessKnight.transform.rotation, default(NetworkPlayer));
				gameObject5.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject5;
			}
			if (ObjectName == "Bishop Cast Iron")
			{
				GameObject gameObject6 = Network.Instantiate(this.ChessBishop, SpawnPos, this.ChessBishop.transform.rotation, default(NetworkPlayer));
				gameObject6.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject6;
			}
			if (ObjectName == "Queen Cast Iron")
			{
				GameObject gameObject7 = Network.Instantiate(this.ChessQueen, SpawnPos, this.ChessQueen.transform.rotation, default(NetworkPlayer));
				gameObject7.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject7;
			}
			if (ObjectName == "King Cast Iron")
			{
				GameObject gameObject8 = Network.Instantiate(this.ChessKing, SpawnPos, this.ChessKing.transform.rotation, default(NetworkPlayer));
				gameObject8.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject8;
			}
			if (ObjectName == "Pawn Light Wood")
			{
				GameObject gameObject9 = Network.Instantiate(this.ChessPawn, SpawnPos, this.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject9.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				return gameObject9;
			}
			if (ObjectName == "Rook Light Wood")
			{
				GameObject gameObject10 = Network.Instantiate(this.ChessRook, SpawnPos, this.ChessRook.transform.rotation, default(NetworkPlayer));
				gameObject10.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				return gameObject10;
			}
			if (ObjectName == "Knight Light Wood")
			{
				GameObject gameObject11 = Network.Instantiate(this.ChessKnight, SpawnPos, this.ChessKnight.transform.rotation, default(NetworkPlayer));
				gameObject11.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				return gameObject11;
			}
			if (ObjectName == "Bishop Light Wood")
			{
				GameObject gameObject12 = Network.Instantiate(this.ChessBishop, SpawnPos, this.ChessBishop.transform.rotation, default(NetworkPlayer));
				gameObject12.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				return gameObject12;
			}
			if (ObjectName == "Queen Light Wood")
			{
				GameObject gameObject13 = Network.Instantiate(this.ChessQueen, SpawnPos, this.ChessQueen.transform.rotation, default(NetworkPlayer));
				gameObject13.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				return gameObject13;
			}
			if (ObjectName == "King Light Wood")
			{
				GameObject gameObject14 = Network.Instantiate(this.ChessKing, SpawnPos, this.ChessKing.transform.rotation, default(NetworkPlayer));
				gameObject14.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				return gameObject14;
			}
			if (ObjectName == "Pawn Dark Wood")
			{
				GameObject gameObject15 = Network.Instantiate(this.ChessPawn, SpawnPos, this.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject15.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				return gameObject15;
			}
			if (ObjectName == "Rook Dark Wood")
			{
				GameObject gameObject16 = Network.Instantiate(this.ChessRook, SpawnPos, this.ChessRook.transform.rotation, default(NetworkPlayer));
				gameObject16.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				return gameObject16;
			}
			if (ObjectName == "Knight Dark Wood")
			{
				GameObject gameObject17 = Network.Instantiate(this.ChessKnight, SpawnPos, this.ChessKnight.transform.rotation, default(NetworkPlayer));
				gameObject17.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				return gameObject17;
			}
			if (ObjectName == "Bishop Dark Wood")
			{
				GameObject gameObject18 = Network.Instantiate(this.ChessBishop, SpawnPos, this.ChessBishop.transform.rotation, default(NetworkPlayer));
				gameObject18.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				return gameObject18;
			}
			if (ObjectName == "Queen Dark Wood")
			{
				GameObject gameObject19 = Network.Instantiate(this.ChessQueen, SpawnPos, this.ChessQueen.transform.rotation, default(NetworkPlayer));
				gameObject19.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				return gameObject19;
			}
			if (ObjectName == "King Dark Wood")
			{
				GameObject gameObject20 = Network.Instantiate(this.ChessKing, SpawnPos, this.ChessKing.transform.rotation, default(NetworkPlayer));
				gameObject20.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				return gameObject20;
			}
			if (ObjectName == "White Ball")
			{
				return Network.Instantiate(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Red Ball")
			{
				GameObject gameObject21 = Network.Instantiate(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
				gameObject21.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject21;
			}
			if (ObjectName == "Yellow Ball")
			{
				GameObject gameObject22 = Network.Instantiate(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
				gameObject22.GetComponent<MaterialSyncScript>().SetMaterial(2);
				return gameObject22;
			}
			if (ObjectName == "Green Ball")
			{
				GameObject gameObject23 = Network.Instantiate(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
				gameObject23.GetComponent<MaterialSyncScript>().SetMaterial(3);
				return gameObject23;
			}
			if (ObjectName == "Blue Ball")
			{
				GameObject gameObject24 = Network.Instantiate(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
				gameObject24.GetComponent<MaterialSyncScript>().SetMaterial(4);
				return gameObject24;
			}
			if (ObjectName == "Pink Ball")
			{
				GameObject gameObject25 = Network.Instantiate(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
				gameObject25.GetComponent<MaterialSyncScript>().SetMaterial(5);
				return gameObject25;
			}
			if (ObjectName == "Black Ball")
			{
				GameObject gameObject26 = Network.Instantiate(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation, default(NetworkPlayer));
				gameObject26.GetComponent<MaterialSyncScript>().SetMaterial(6);
				return gameObject26;
			}
			if (ObjectName == "Metal Ball")
			{
				return Network.Instantiate(this.MetalBall, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "D4")
			{
				return Network.Instantiate(this.D4, SpawnPos, this.D4.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "D6")
			{
				return Network.Instantiate(this.D6, SpawnPos, this.D6.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "D8")
			{
				return Network.Instantiate(this.D8, SpawnPos, this.D8.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "D10")
			{
				return Network.Instantiate(this.D10, SpawnPos, this.D10.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "D12")
			{
				return Network.Instantiate(this.D12, SpawnPos, this.D12.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "D20")
			{
				return Network.Instantiate(this.D20, SpawnPos, this.D20.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "D6 Black")
			{
				return Network.Instantiate(this.D6Rounded, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "D6 Red")
			{
				GameObject gameObject27 = Network.Instantiate(this.D6Rounded, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject27.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject27;
			}
			if (ObjectName == "D6 Green")
			{
				GameObject gameObject28 = Network.Instantiate(this.D6Rounded, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject28.GetComponent<MaterialSyncScript>().SetMaterial(2);
				return gameObject28;
			}
			if (ObjectName == "D6 Blue")
			{
				GameObject gameObject29 = Network.Instantiate(this.D6Rounded, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject29.GetComponent<MaterialSyncScript>().SetMaterial(3);
				return gameObject29;
			}
			if (ObjectName == "D4 Chrome")
			{
				GameObject gameObject30 = Network.Instantiate(this.D4, SpawnPos, this.D4.transform.rotation, default(NetworkPlayer));
				gameObject30.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				return gameObject30;
			}
			if (ObjectName == "D6 Chrome")
			{
				GameObject gameObject31 = Network.Instantiate(this.D6, SpawnPos, this.D6.transform.rotation, default(NetworkPlayer));
				gameObject31.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				return gameObject31;
			}
			if (ObjectName == "D8 Chrome")
			{
				GameObject gameObject32 = Network.Instantiate(this.D8, SpawnPos, this.D8.transform.rotation, default(NetworkPlayer));
				gameObject32.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				return gameObject32;
			}
			if (ObjectName == "D10 Chrome")
			{
				GameObject gameObject33 = Network.Instantiate(this.D10, SpawnPos, this.D10.transform.rotation, default(NetworkPlayer));
				gameObject33.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				return gameObject33;
			}
			if (ObjectName == "D12 Chrome")
			{
				GameObject gameObject34 = Network.Instantiate(this.D12, SpawnPos, this.D12.transform.rotation, default(NetworkPlayer));
				gameObject34.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				return gameObject34;
			}
			if (ObjectName == "D20 Chrome")
			{
				GameObject gameObject35 = Network.Instantiate(this.D20, SpawnPos, this.D20.transform.rotation, default(NetworkPlayer));
				gameObject35.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				return gameObject35;
			}
			if (ObjectName == "GO Piece White")
			{
				return Network.Instantiate(this.GoPieceWhite, SpawnPos, this.GoPieceWhite.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "GO Piece Black")
			{
				return Network.Instantiate(this.GoPieceBlack, SpawnPos, this.GoPieceBlack.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "GO Bowl White")
			{
				return Network.Instantiate(this.GoBowlWhite, SpawnPos, this.GoBowlWhite.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "GO Bowl Black")
			{
				return Network.Instantiate(this.GoBowlBlack, SpawnPos, this.GoBowlBlack.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "White Pawn")
			{
				return Network.Instantiate(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Red Pawn")
			{
				GameObject gameObject36 = Network.Instantiate(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject36.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject36;
			}
			if (ObjectName == "Orange Pawn")
			{
				GameObject gameObject37 = Network.Instantiate(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject37.GetComponent<MaterialSyncScript>().SetMaterial(2);
				return gameObject37;
			}
			if (ObjectName == "Yellow Pawn")
			{
				GameObject gameObject38 = Network.Instantiate(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject38.GetComponent<MaterialSyncScript>().SetMaterial(3);
				return gameObject38;
			}
			if (ObjectName == "Green Pawn")
			{
				GameObject gameObject39 = Network.Instantiate(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject39.GetComponent<MaterialSyncScript>().SetMaterial(4);
				return gameObject39;
			}
			if (ObjectName == "Blue Pawn")
			{
				GameObject gameObject40 = Network.Instantiate(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject40.GetComponent<MaterialSyncScript>().SetMaterial(5);
				return gameObject40;
			}
			if (ObjectName == "Purple Pawn")
			{
				GameObject gameObject41 = Network.Instantiate(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject41.GetComponent<MaterialSyncScript>().SetMaterial(6);
				return gameObject41;
			}
			if (ObjectName == "Pink Pawn")
			{
				GameObject gameObject42 = Network.Instantiate(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject42.GetComponent<MaterialSyncScript>().SetMaterial(7);
				return gameObject42;
			}
			if (ObjectName == "Black Pawn")
			{
				GameObject gameObject43 = Network.Instantiate(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation, default(NetworkPlayer));
				gameObject43.GetComponent<MaterialSyncScript>().SetMaterial(8);
				return gameObject43;
			}
			if (ObjectName == "Blue 10")
			{
				return Network.Instantiate(this.Poker10, SpawnPos, this.Poker10.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Green 50")
			{
				return Network.Instantiate(this.Poker50, SpawnPos, this.Poker50.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Red 100")
			{
				return Network.Instantiate(this.Poker100, SpawnPos, this.Poker100.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Silver 500")
			{
				return Network.Instantiate(this.Poker500, SpawnPos, this.Poker500.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Gold 1000")
			{
				return Network.Instantiate(this.Poker1000, SpawnPos, this.Poker1000.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Custom Model")
			{
				return Network.Instantiate(this.CustomModel, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Loot Bag")
			{
				return Network.Instantiate(this.Bag, SpawnPos, this.Bag.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Quarter")
			{
				return Network.Instantiate(this.D2, SpawnPos, this.D2.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Reversi Chip")
			{
				return Network.Instantiate(this.ReversiChip, SpawnPos, this.ReversiChip.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Random Domino")
			{
				GameObject gameObject44 = Network.Instantiate(this.Domino, SpawnPos, this.Domino.transform.rotation, default(NetworkPlayer));
				gameObject44.GetComponent<MeshSyncScript>().SetMesh(UnityEngine.Random.Range(0, 27));
				return gameObject44;
			}
			if (ObjectName == "Random Mahjong")
			{
				GameObject gameObject45 = Network.Instantiate(this.MahjongTile, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject45.GetComponent<MeshSyncScript>().SetMesh(UnityEngine.Random.Range(0, 35));
				return gameObject45;
			}
			if (ObjectName == "Brown Backgammon")
			{
				return Network.Instantiate(this.BackgammonPieceBrown, SpawnPos, this.BackgammonPieceBrown.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "White Backgammon")
			{
				return Network.Instantiate(this.BackgammonPieceWhite, SpawnPos, this.BackgammonPieceWhite.transform.rotation, default(NetworkPlayer));
			}
			if (ObjectName == "Arms Dice")
			{
				GameObject gameObject46 = Network.Instantiate(this.DiePiecepack, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject46.GetComponent<MaterialSyncScript>().SetMaterial(0);
				return gameObject46;
			}
			if (ObjectName == "Crowns Dice")
			{
				GameObject gameObject47 = Network.Instantiate(this.DiePiecepack, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject47.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject47;
			}
			if (ObjectName == "Moons Dice")
			{
				GameObject gameObject48 = Network.Instantiate(this.DiePiecepack, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject48.GetComponent<MaterialSyncScript>().SetMaterial(2);
				return gameObject48;
			}
			if (ObjectName == "Suns Dice")
			{
				GameObject gameObject49 = Network.Instantiate(this.DiePiecepack, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject49.GetComponent<MaterialSyncScript>().SetMaterial(3);
				return gameObject49;
			}
			if (ObjectName == "Coin Blank")
			{
				GameObject gameObject50 = Network.Instantiate(this.Piecepackpiece, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject50.GetComponent<MeshSyncScript>().SetMesh(0);
				return gameObject50;
			}
			if (ObjectName == "Coin Ace")
			{
				GameObject gameObject51 = Network.Instantiate(this.Piecepackpiece, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject51.GetComponent<MeshSyncScript>().SetMesh(1);
				return gameObject51;
			}
			if (ObjectName == "Coin 2")
			{
				GameObject gameObject52 = Network.Instantiate(this.Piecepackpiece, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject52.GetComponent<MeshSyncScript>().SetMesh(2);
				return gameObject52;
			}
			if (ObjectName == "Coin 3")
			{
				GameObject gameObject53 = Network.Instantiate(this.Piecepackpiece, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject53.GetComponent<MeshSyncScript>().SetMesh(3);
				return gameObject53;
			}
			if (ObjectName == "Coin 4")
			{
				GameObject gameObject54 = Network.Instantiate(this.Piecepackpiece, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject54.GetComponent<MeshSyncScript>().SetMesh(4);
				return gameObject54;
			}
			if (ObjectName == "Coin 5")
			{
				GameObject gameObject55 = Network.Instantiate(this.Piecepackpiece, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject55.GetComponent<MeshSyncScript>().SetMesh(5);
				return gameObject55;
			}
			if (ObjectName == "Square Blank")
			{
				GameObject gameObject56 = Network.Instantiate(this.Piecepackpiece, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject56.GetComponent<MeshSyncScript>().SetMesh(6);
				return gameObject56;
			}
			if (ObjectName == "Square Ace")
			{
				GameObject gameObject57 = Network.Instantiate(this.Piecepackpiece, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject57.GetComponent<MeshSyncScript>().SetMesh(7);
				return gameObject57;
			}
			if (ObjectName == "Square 2")
			{
				GameObject gameObject58 = Network.Instantiate(this.Piecepackpiece, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject58.GetComponent<MeshSyncScript>().SetMesh(8);
				return gameObject58;
			}
			if (ObjectName == "Square 3")
			{
				GameObject gameObject59 = Network.Instantiate(this.Piecepackpiece, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject59.GetComponent<MeshSyncScript>().SetMesh(9);
				return gameObject59;
			}
			if (ObjectName == "Square 4")
			{
				GameObject gameObject60 = Network.Instantiate(this.Piecepackpiece, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject60.GetComponent<MeshSyncScript>().SetMesh(10);
				return gameObject60;
			}
			if (ObjectName == "Square 5")
			{
				GameObject gameObject61 = Network.Instantiate(this.Piecepackpiece, SpawnPos, Quaternion.identity, default(NetworkPlayer));
				gameObject61.GetComponent<MeshSyncScript>().SetMesh(11);
				return gameObject61;
			}
			if (ObjectName == "Bear")
			{
				return Network.Instantiate(this.rpgBEAR, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Chimera")
			{
				return Network.Instantiate(this.rpgCHIMERA, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Cyclops")
			{
				return Network.Instantiate(this.rpgCYCLOPS, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Dragonide")
			{
				return Network.Instantiate(this.rpgDRAGONIDE, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Evil Watcher")
			{
				return Network.Instantiate(this.rpgEVILWATCHER, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Giant Rat")
			{
				return Network.Instantiate(this.rpgRAT, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Giant Viper")
			{
				return Network.Instantiate(this.rpgGIANTVIPER, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Goblin")
			{
				return Network.Instantiate(this.rpgGOBLIN, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Golem")
			{
				return Network.Instantiate(this.rpgGOLEM, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Ghoul")
			{
				return Network.Instantiate(this.rpgGHOUL, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Griffon")
			{
				return Network.Instantiate(this.rpgGRIFFON, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Hydra")
			{
				return Network.Instantiate(this.rpgHYDRA, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Kobold")
			{
				return Network.Instantiate(this.rpgKOBOLD, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Lizard Warrior")
			{
				return Network.Instantiate(this.rpgLIZARDWARRIOR, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Manticora")
			{
				return Network.Instantiate(this.rpgMANTICORA, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Mummy")
			{
				return Network.Instantiate(this.rpgMUMMY, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Ogre")
			{
				return Network.Instantiate(this.rpgOGRE, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Orc")
			{
				return Network.Instantiate(this.rpgORC, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Skeleton Knight")
			{
				return Network.Instantiate(this.rpgSKELETONKNIGHT, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Tree Ent")
			{
				return Network.Instantiate(this.rpgTREEENT, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Troll")
			{
				return Network.Instantiate(this.rpgTROLL, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Werewolf")
			{
				return Network.Instantiate(this.rpgWEREWOLF, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Wolf")
			{
				return Network.Instantiate(this.rpgWOLF, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Wyvern")
			{
				return Network.Instantiate(this.rpgWYVERN, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Vampire")
			{
				return Network.Instantiate(this.rpgVAMPIRE, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Bear")
			{
				return Network.Instantiate(this.rpgBEAR, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Giant Rat")
			{
				return Network.Instantiate(this.rpgRAT, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Giant Viper")
			{
				return Network.Instantiate(this.rpgGIANTVIPER, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Wolf")
			{
				return Network.Instantiate(this.rpgWOLF, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Evil Watcher")
			{
				return Network.Instantiate(this.rpgEVILWATCHER, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Golem")
			{
				return Network.Instantiate(this.rpgGOLEM, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Tree Ent")
			{
				return Network.Instantiate(this.rpgTREEENT, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Dragonide")
			{
				return Network.Instantiate(this.rpgDRAGONIDE, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Hydra")
			{
				return Network.Instantiate(this.rpgHYDRA, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Lizard Warrior")
			{
				return Network.Instantiate(this.rpgLIZARDWARRIOR, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Wyvern")
			{
				return Network.Instantiate(this.rpgWYVERN, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Ghoul")
			{
				return Network.Instantiate(this.rpgGHOUL, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Mummy")
			{
				return Network.Instantiate(this.rpgMUMMY, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Skeleton Knight")
			{
				return Network.Instantiate(this.rpgSKELETONKNIGHT, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Vampire")
			{
				return Network.Instantiate(this.rpgVAMPIRE, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Cyclops")
			{
				return Network.Instantiate(this.rpgCYCLOPS, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Goblin")
			{
				return Network.Instantiate(this.rpgGOBLIN, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Kobold")
			{
				return Network.Instantiate(this.rpgKOBOLD, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Ogre")
			{
				return Network.Instantiate(this.rpgOGRE, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Orc")
			{
				return Network.Instantiate(this.rpgORC, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Troll")
			{
				return Network.Instantiate(this.rpgTROLL, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Chimera")
			{
				return Network.Instantiate(this.rpgCHIMERA, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Griffon")
			{
				return Network.Instantiate(this.rpgGRIFFON, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Manticora")
			{
				return Network.Instantiate(this.rpgMANTICORA, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Werewolf")
			{
				return Network.Instantiate(this.rpgWEREWOLF, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Floor")
			{
				return Network.Instantiate(this.TilesetFloor, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Wall")
			{
				return Network.Instantiate(this.TilesetWall, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Corner")
			{
				return Network.Instantiate(this.TilesetCorner, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Chest")
			{
				return Network.Instantiate(this.TilesetChest, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Barrel")
			{
				return Network.Instantiate(this.TilesetBarrel, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Table")
			{
				return Network.Instantiate(this.TilesetTable, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Chair")
			{
				return Network.Instantiate(this.TilesetChair, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Tree")
			{
				return Network.Instantiate(this.TilesetTree, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Rock")
			{
				return Network.Instantiate(this.TilesetRock, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Custom Figurine")
			{
				return Network.Instantiate(this.CustomFigurine, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Knight of Knil")
			{
				return Network.Instantiate(this.FigurineKnil, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Kimi Kat")
			{
				return Network.Instantiate(this.FigurineKimiKats, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Sir Loin")
			{
				return Network.Instantiate(this.FigurineLoin, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Mara")
			{
				return Network.Instantiate(this.FigurineMara, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Zomblor")
			{
				return Network.Instantiate(this.FigurineZomblor, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Zeke Kodoku")
			{
				return Network.Instantiate(this.FigurineZeke, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "CardBot")
			{
				return Network.Instantiate(this.FigurineCardBot, SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Backgammon Board")
			{
				string name = "backgammon_board";
				return Network.Instantiate(this.GetPrefab(name), SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Checkers Board")
			{
				string name2 = "Checker_Board";
				return Network.Instantiate(this.GetPrefab(name2), SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Chess Board")
			{
				string name3 = "Chess_Board";
				return Network.Instantiate(this.GetPrefab(name3), SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Chinese Checkers Board")
			{
				string name4 = "Chinese_Checkers_Board";
				return Network.Instantiate(this.GetPrefab(name4), SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Custom Board")
			{
				string name5 = "Custom_Board";
				return Network.Instantiate(this.GetPrefab(name5), SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Go Board")
			{
				string name6 = "Go_Board";
				return Network.Instantiate(this.GetPrefab(name6), SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Pachisi Board")
			{
				string name7 = "Pachisi_board";
				return Network.Instantiate(this.GetPrefab(name7), SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			if (ObjectName == "Reversi Board")
			{
				string name8 = "reversi_board";
				return Network.Instantiate(this.GetPrefab(name8), SpawnPos, Quaternion.identity, default(NetworkPlayer));
			}
			GameObject prefab = this.GetPrefab(ObjectName);
			if (prefab)
			{
				return Network.Instantiate(prefab, SpawnPos, prefab.transform.rotation, default(NetworkPlayer));
			}
		}
		else
		{
			if (ObjectName == "Tablet")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.Tablet, SpawnPos, this.Tablet.transform.rotation);
			}
			if (ObjectName == "Digital Clock")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.DigitalClock, SpawnPos, this.DigitalClock.transform.rotation);
			}
			if (ObjectName == "Red Square")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.BlockSquare, SpawnPos, this.BlockSquare.transform.rotation);
			}
			if (ObjectName == "Blue Rectangle")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.BlockRectangle, SpawnPos, this.BlockRectangle.transform.rotation);
			}
			if (ObjectName == "Green Triangle")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.BlockTriangle, SpawnPos, this.BlockTriangle.transform.rotation);
			}
			if (ObjectName == "Standard Deck")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.Deck, SpawnPos, this.Deck.transform.rotation);
			}
			if (ObjectName == "Custom Deck")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.DeckCustom, SpawnPos, this.Deck.transform.rotation);
			}
			if (ObjectName == "Random Card")
			{
				GameObject gameObject62 = UnityEngine.Object.Instantiate<GameObject>(this.Card, SpawnPos, this.Card.transform.rotation);
				int num2 = UnityEngine.Random.Range(0, 51);
				NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject62, num2, -1, false);
				gameObject62.GetComponent<CardScript>().card_id_ = num2;
				return gameObject62;
			}
			if (ObjectName == "Joker")
			{
				GameObject gameObject63 = UnityEngine.Object.Instantiate<GameObject>(this.Card, SpawnPos, this.Card.transform.rotation);
				NetworkSingleton<CardManagerScript>.Instance.SetupCard(gameObject63, 52, -1, false);
				gameObject63.GetComponent<CardScript>().card_id_ = 52;
				return gameObject63;
			}
			if (ObjectName == "CardBots Main Deck")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.DeckCardBotMain, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "CardBots Head Deck")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.DeckCardBotHead, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Red Checker")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.CheckerRed, SpawnPos, this.CheckerRed.transform.rotation);
			}
			if (ObjectName == "Black Checker")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.CheckerBlack, SpawnPos, this.CheckerBlack.transform.rotation);
			}
			if (ObjectName == "White Checker")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.CheckerWhite, SpawnPos, this.CheckerWhite.transform.rotation);
			}
			if (ObjectName == "Pawn Chrome")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.ChessPawn, SpawnPos, this.ChessPawn.transform.rotation);
			}
			if (ObjectName == "Rook Chrome")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.ChessRook, SpawnPos, this.ChessRook.transform.rotation);
			}
			if (ObjectName == "Knight Chrome")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.ChessKnight, SpawnPos, this.ChessKnight.transform.rotation);
			}
			if (ObjectName == "Bishop Chrome")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.ChessBishop, SpawnPos, this.ChessBishop.transform.rotation);
			}
			if (ObjectName == "Queen Chrome")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.ChessQueen, SpawnPos, this.ChessQueen.transform.rotation);
			}
			if (ObjectName == "King Chrome")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.ChessKing, SpawnPos, this.ChessKing.transform.rotation);
			}
			if (ObjectName == "Pawn Cast Iron")
			{
				GameObject gameObject64 = UnityEngine.Object.Instantiate<GameObject>(this.ChessPawn, SpawnPos, this.ChessPawn.transform.rotation);
				gameObject64.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject64;
			}
			if (ObjectName == "Rook Cast Iron")
			{
				GameObject gameObject65 = UnityEngine.Object.Instantiate<GameObject>(this.ChessRook, SpawnPos, this.ChessRook.transform.rotation);
				gameObject65.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject65;
			}
			if (ObjectName == "Knight Cast Iron")
			{
				GameObject gameObject66 = UnityEngine.Object.Instantiate<GameObject>(this.ChessKnight, SpawnPos, this.ChessKnight.transform.rotation);
				gameObject66.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject66;
			}
			if (ObjectName == "Bishop Cast Iron")
			{
				GameObject gameObject67 = UnityEngine.Object.Instantiate<GameObject>(this.ChessBishop, SpawnPos, this.ChessBishop.transform.rotation);
				gameObject67.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject67;
			}
			if (ObjectName == "Queen Cast Iron")
			{
				GameObject gameObject68 = UnityEngine.Object.Instantiate<GameObject>(this.ChessQueen, SpawnPos, this.ChessQueen.transform.rotation);
				gameObject68.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject68;
			}
			if (ObjectName == "King Cast Iron")
			{
				GameObject gameObject69 = UnityEngine.Object.Instantiate<GameObject>(this.ChessKing, SpawnPos, this.ChessKing.transform.rotation);
				gameObject69.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject69;
			}
			if (ObjectName == "Pawn Light Wood")
			{
				GameObject gameObject70 = UnityEngine.Object.Instantiate<GameObject>(this.ChessPawn, SpawnPos, this.ChessPawn.transform.rotation);
				gameObject70.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				return gameObject70;
			}
			if (ObjectName == "Rook Light Wood")
			{
				GameObject gameObject71 = UnityEngine.Object.Instantiate<GameObject>(this.ChessRook, SpawnPos, this.ChessRook.transform.rotation);
				gameObject71.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				return gameObject71;
			}
			if (ObjectName == "Knight Light Wood")
			{
				GameObject gameObject72 = UnityEngine.Object.Instantiate<GameObject>(this.ChessKnight, SpawnPos, this.ChessKnight.transform.rotation);
				gameObject72.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				return gameObject72;
			}
			if (ObjectName == "Bishop Light Wood")
			{
				GameObject gameObject73 = UnityEngine.Object.Instantiate<GameObject>(this.ChessBishop, SpawnPos, this.ChessBishop.transform.rotation);
				gameObject73.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				return gameObject73;
			}
			if (ObjectName == "Queen Light Wood")
			{
				GameObject gameObject74 = UnityEngine.Object.Instantiate<GameObject>(this.ChessQueen, SpawnPos, this.ChessQueen.transform.rotation);
				gameObject74.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				return gameObject74;
			}
			if (ObjectName == "King Light Wood")
			{
				GameObject gameObject75 = UnityEngine.Object.Instantiate<GameObject>(this.ChessKing, SpawnPos, this.ChessKing.transform.rotation);
				gameObject75.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 2);
				return gameObject75;
			}
			if (ObjectName == "Pawn Dark Wood")
			{
				GameObject gameObject76 = UnityEngine.Object.Instantiate<GameObject>(this.ChessPawn, SpawnPos, this.ChessPawn.transform.rotation);
				gameObject76.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				return gameObject76;
			}
			if (ObjectName == "Rook Dark Wood")
			{
				GameObject gameObject77 = UnityEngine.Object.Instantiate<GameObject>(this.ChessRook, SpawnPos, this.ChessRook.transform.rotation);
				gameObject77.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				return gameObject77;
			}
			if (ObjectName == "Knight Dark Wood")
			{
				GameObject gameObject78 = UnityEngine.Object.Instantiate<GameObject>(this.ChessKnight, SpawnPos, this.ChessKnight.transform.rotation);
				gameObject78.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				return gameObject78;
			}
			if (ObjectName == "Bishop Dark Wood")
			{
				GameObject gameObject79 = UnityEngine.Object.Instantiate<GameObject>(this.ChessBishop, SpawnPos, this.ChessBishop.transform.rotation);
				gameObject79.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				return gameObject79;
			}
			if (ObjectName == "Queen Dark Wood")
			{
				GameObject gameObject80 = UnityEngine.Object.Instantiate<GameObject>(this.ChessQueen, SpawnPos, this.ChessQueen.transform.rotation);
				gameObject80.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				return gameObject80;
			}
			if (ObjectName == "King Dark Wood")
			{
				GameObject gameObject81 = UnityEngine.Object.Instantiate<GameObject>(this.ChessKing, SpawnPos, this.ChessKing.transform.rotation);
				gameObject81.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 3);
				return gameObject81;
			}
			if (ObjectName == "White Ball")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation);
			}
			if (ObjectName == "Red Ball")
			{
				GameObject gameObject82 = UnityEngine.Object.Instantiate<GameObject>(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation);
				gameObject82.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject82;
			}
			if (ObjectName == "Yellow Ball")
			{
				GameObject gameObject83 = UnityEngine.Object.Instantiate<GameObject>(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation);
				gameObject83.GetComponent<MaterialSyncScript>().SetMaterial(2);
				return gameObject83;
			}
			if (ObjectName == "Green Ball")
			{
				GameObject gameObject84 = UnityEngine.Object.Instantiate<GameObject>(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation);
				gameObject84.GetComponent<MaterialSyncScript>().SetMaterial(3);
				return gameObject84;
			}
			if (ObjectName == "Blue Ball")
			{
				GameObject gameObject85 = UnityEngine.Object.Instantiate<GameObject>(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation);
				gameObject85.GetComponent<MaterialSyncScript>().SetMaterial(4);
				return gameObject85;
			}
			if (ObjectName == "Pink Ball")
			{
				GameObject gameObject86 = UnityEngine.Object.Instantiate<GameObject>(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation);
				gameObject86.GetComponent<MaterialSyncScript>().SetMaterial(5);
				return gameObject86;
			}
			if (ObjectName == "Black Ball")
			{
				GameObject gameObject87 = UnityEngine.Object.Instantiate<GameObject>(this.ChineseCheckersPiece, SpawnPos, this.ChineseCheckersPiece.transform.rotation);
				gameObject87.GetComponent<MaterialSyncScript>().SetMaterial(6);
				return gameObject87;
			}
			if (ObjectName == "Metal Ball")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.MetalBall, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "D4")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.D4, SpawnPos, this.D4.transform.rotation);
			}
			if (ObjectName == "D6")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.D6, SpawnPos, this.D6.transform.rotation);
			}
			if (ObjectName == "D8")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.D8, SpawnPos, this.D8.transform.rotation);
			}
			if (ObjectName == "D10")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.D10, SpawnPos, this.D10.transform.rotation);
			}
			if (ObjectName == "D12")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.D12, SpawnPos, this.D12.transform.rotation);
			}
			if (ObjectName == "D20")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.D20, SpawnPos, this.D20.transform.rotation);
			}
			if (ObjectName == "D6 Black")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.D6Rounded, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "D6 Red")
			{
				GameObject gameObject88 = UnityEngine.Object.Instantiate<GameObject>(this.D6Rounded, SpawnPos, Quaternion.identity);
				gameObject88.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject88;
			}
			if (ObjectName == "D6 Green")
			{
				GameObject gameObject89 = UnityEngine.Object.Instantiate<GameObject>(this.D6Rounded, SpawnPos, Quaternion.identity);
				gameObject89.GetComponent<MaterialSyncScript>().SetMaterial(2);
				return gameObject89;
			}
			if (ObjectName == "D6 Blue")
			{
				GameObject gameObject90 = UnityEngine.Object.Instantiate<GameObject>(this.D6Rounded, SpawnPos, Quaternion.identity);
				gameObject90.GetComponent<MaterialSyncScript>().SetMaterial(3);
				return gameObject90;
			}
			if (ObjectName == "D4 Chrome")
			{
				GameObject gameObject91 = UnityEngine.Object.Instantiate<GameObject>(this.D4, SpawnPos, this.D4.transform.rotation);
				gameObject91.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				return gameObject91;
			}
			if (ObjectName == "D6 Chrome")
			{
				GameObject gameObject92 = UnityEngine.Object.Instantiate<GameObject>(this.D6, SpawnPos, this.D6.transform.rotation);
				gameObject92.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				return gameObject92;
			}
			if (ObjectName == "D8 Chrome")
			{
				GameObject gameObject93 = UnityEngine.Object.Instantiate<GameObject>(this.D8, SpawnPos, this.D8.transform.rotation);
				gameObject93.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				return gameObject93;
			}
			if (ObjectName == "D10 Chrome")
			{
				GameObject gameObject94 = UnityEngine.Object.Instantiate<GameObject>(this.D10, SpawnPos, this.D10.transform.rotation);
				gameObject94.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				return gameObject94;
			}
			if (ObjectName == "D12 Chrome")
			{
				GameObject gameObject95 = UnityEngine.Object.Instantiate<GameObject>(this.D12, SpawnPos, this.D12.transform.rotation);
				gameObject95.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				return gameObject95;
			}
			if (ObjectName == "D20 Chrome")
			{
				GameObject gameObject96 = UnityEngine.Object.Instantiate<GameObject>(this.D20, SpawnPos, this.D20.transform.rotation);
				gameObject96.GetComponent<NetworkPhysicsObject>().SetObject(true, -1, 1);
				return gameObject96;
			}
			if (ObjectName == "GO Piece White")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.GoPieceWhite, SpawnPos, this.GoPieceWhite.transform.rotation);
			}
			if (ObjectName == "GO Piece Black")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.GoPieceBlack, SpawnPos, this.GoPieceBlack.transform.rotation);
			}
			if (ObjectName == "GO Bowl White")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.GoBowlWhite, SpawnPos, this.GoBowlWhite.transform.rotation);
			}
			if (ObjectName == "GO Bowl Black")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.GoBowlBlack, SpawnPos, this.GoBowlBlack.transform.rotation);
			}
			if (ObjectName == "White Pawn")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation);
			}
			if (ObjectName == "Red Pawn")
			{
				GameObject gameObject97 = UnityEngine.Object.Instantiate<GameObject>(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation);
				gameObject97.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject97;
			}
			if (ObjectName == "Orange Pawn")
			{
				GameObject gameObject98 = UnityEngine.Object.Instantiate<GameObject>(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation);
				gameObject98.GetComponent<MaterialSyncScript>().SetMaterial(2);
				return gameObject98;
			}
			if (ObjectName == "Yellow Pawn")
			{
				GameObject gameObject99 = UnityEngine.Object.Instantiate<GameObject>(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation);
				gameObject99.GetComponent<MaterialSyncScript>().SetMaterial(3);
				return gameObject99;
			}
			if (ObjectName == "Green Pawn")
			{
				GameObject gameObject100 = UnityEngine.Object.Instantiate<GameObject>(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation);
				gameObject100.GetComponent<MaterialSyncScript>().SetMaterial(4);
				return gameObject100;
			}
			if (ObjectName == "Blue Pawn")
			{
				GameObject gameObject101 = UnityEngine.Object.Instantiate<GameObject>(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation);
				gameObject101.GetComponent<MaterialSyncScript>().SetMaterial(5);
				return gameObject101;
			}
			if (ObjectName == "Purple Pawn")
			{
				GameObject gameObject102 = UnityEngine.Object.Instantiate<GameObject>(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation);
				gameObject102.GetComponent<MaterialSyncScript>().SetMaterial(6);
				return gameObject102;
			}
			if (ObjectName == "Pink Pawn")
			{
				GameObject gameObject103 = UnityEngine.Object.Instantiate<GameObject>(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation);
				gameObject103.GetComponent<MaterialSyncScript>().SetMaterial(7);
				return gameObject103;
			}
			if (ObjectName == "Black Pawn")
			{
				GameObject gameObject104 = UnityEngine.Object.Instantiate<GameObject>(this.PlayerPawn, SpawnPos, this.ChessPawn.transform.rotation);
				gameObject104.GetComponent<MaterialSyncScript>().SetMaterial(8);
				return gameObject104;
			}
			if (ObjectName == "Blue 10")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.Poker10, SpawnPos, this.Poker10.transform.rotation);
			}
			if (ObjectName == "Green 50")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.Poker50, SpawnPos, this.Poker50.transform.rotation);
			}
			if (ObjectName == "Red 100")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.Poker100, SpawnPos, this.Poker100.transform.rotation);
			}
			if (ObjectName == "Silver 500")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.Poker500, SpawnPos, this.Poker500.transform.rotation);
			}
			if (ObjectName == "Gold 1000")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.Poker1000, SpawnPos, this.Poker1000.transform.rotation);
			}
			if (ObjectName == "Custom Model")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.CustomModel, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Loot Bag")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.Bag, SpawnPos, this.Bag.transform.rotation);
			}
			if (ObjectName == "Quarter")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.D2, SpawnPos, this.D2.transform.rotation);
			}
			if (ObjectName == "Reversi Chip")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.ReversiChip, SpawnPos, this.ReversiChip.transform.rotation);
			}
			if (ObjectName == "Random Domino")
			{
				GameObject gameObject105 = UnityEngine.Object.Instantiate<GameObject>(this.Domino, SpawnPos, this.Domino.transform.rotation);
				gameObject105.GetComponent<MeshSyncScript>().SetMesh(UnityEngine.Random.Range(0, 27));
				return gameObject105;
			}
			if (ObjectName == "Random Mahjong")
			{
				GameObject gameObject106 = UnityEngine.Object.Instantiate<GameObject>(this.MahjongTile, SpawnPos, Quaternion.identity);
				gameObject106.GetComponent<MeshSyncScript>().SetMesh(UnityEngine.Random.Range(0, 35));
				return gameObject106;
			}
			if (ObjectName == "Brown Backgammon")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.BackgammonPieceBrown, SpawnPos, this.BackgammonPieceBrown.transform.rotation);
			}
			if (ObjectName == "White Backgammon")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.BackgammonPieceWhite, SpawnPos, this.BackgammonPieceWhite.transform.rotation);
			}
			if (ObjectName == "Arms Dice")
			{
				GameObject gameObject107 = UnityEngine.Object.Instantiate<GameObject>(this.DiePiecepack, SpawnPos, Quaternion.identity);
				gameObject107.GetComponent<MaterialSyncScript>().SetMaterial(0);
				return gameObject107;
			}
			if (ObjectName == "Crowns Dice")
			{
				GameObject gameObject108 = UnityEngine.Object.Instantiate<GameObject>(this.DiePiecepack, SpawnPos, Quaternion.identity);
				gameObject108.GetComponent<MaterialSyncScript>().SetMaterial(1);
				return gameObject108;
			}
			if (ObjectName == "Moons Dice")
			{
				GameObject gameObject109 = UnityEngine.Object.Instantiate<GameObject>(this.DiePiecepack, SpawnPos, Quaternion.identity);
				gameObject109.GetComponent<MaterialSyncScript>().SetMaterial(2);
				return gameObject109;
			}
			if (ObjectName == "Suns Dice")
			{
				GameObject gameObject110 = UnityEngine.Object.Instantiate<GameObject>(this.DiePiecepack, SpawnPos, Quaternion.identity);
				gameObject110.GetComponent<MaterialSyncScript>().SetMaterial(3);
				return gameObject110;
			}
			if (ObjectName == "Coin Blank")
			{
				GameObject gameObject111 = UnityEngine.Object.Instantiate<GameObject>(this.Piecepackpiece, SpawnPos, Quaternion.identity);
				gameObject111.GetComponent<MeshSyncScript>().SetMesh(0);
				return gameObject111;
			}
			if (ObjectName == "Coin Ace")
			{
				GameObject gameObject112 = UnityEngine.Object.Instantiate<GameObject>(this.Piecepackpiece, SpawnPos, Quaternion.identity);
				gameObject112.GetComponent<MeshSyncScript>().SetMesh(1);
				return gameObject112;
			}
			if (ObjectName == "Coin 2")
			{
				GameObject gameObject113 = UnityEngine.Object.Instantiate<GameObject>(this.Piecepackpiece, SpawnPos, Quaternion.identity);
				gameObject113.GetComponent<MeshSyncScript>().SetMesh(2);
				return gameObject113;
			}
			if (ObjectName == "Coin 3")
			{
				GameObject gameObject114 = UnityEngine.Object.Instantiate<GameObject>(this.Piecepackpiece, SpawnPos, Quaternion.identity);
				gameObject114.GetComponent<MeshSyncScript>().SetMesh(3);
				return gameObject114;
			}
			if (ObjectName == "Coin 4")
			{
				GameObject gameObject115 = UnityEngine.Object.Instantiate<GameObject>(this.Piecepackpiece, SpawnPos, Quaternion.identity);
				gameObject115.GetComponent<MeshSyncScript>().SetMesh(4);
				return gameObject115;
			}
			if (ObjectName == "Coin 5")
			{
				GameObject gameObject116 = UnityEngine.Object.Instantiate<GameObject>(this.Piecepackpiece, SpawnPos, Quaternion.identity);
				gameObject116.GetComponent<MeshSyncScript>().SetMesh(5);
				return gameObject116;
			}
			if (ObjectName == "Square Blank")
			{
				GameObject gameObject117 = UnityEngine.Object.Instantiate<GameObject>(this.Piecepackpiece, SpawnPos, Quaternion.identity);
				gameObject117.GetComponent<MeshSyncScript>().SetMesh(6);
				return gameObject117;
			}
			if (ObjectName == "Square Ace")
			{
				GameObject gameObject118 = UnityEngine.Object.Instantiate<GameObject>(this.Piecepackpiece, SpawnPos, Quaternion.identity);
				gameObject118.GetComponent<MeshSyncScript>().SetMesh(7);
				return gameObject118;
			}
			if (ObjectName == "Square 2")
			{
				GameObject gameObject119 = UnityEngine.Object.Instantiate<GameObject>(this.Piecepackpiece, SpawnPos, Quaternion.identity);
				gameObject119.GetComponent<MeshSyncScript>().SetMesh(8);
				return gameObject119;
			}
			if (ObjectName == "Square 3")
			{
				GameObject gameObject120 = UnityEngine.Object.Instantiate<GameObject>(this.Piecepackpiece, SpawnPos, Quaternion.identity);
				gameObject120.GetComponent<MeshSyncScript>().SetMesh(9);
				return gameObject120;
			}
			if (ObjectName == "Square 4")
			{
				GameObject gameObject121 = UnityEngine.Object.Instantiate<GameObject>(this.Piecepackpiece, SpawnPos, Quaternion.identity);
				gameObject121.GetComponent<MeshSyncScript>().SetMesh(10);
				return gameObject121;
			}
			if (ObjectName == "Square 5")
			{
				GameObject gameObject122 = UnityEngine.Object.Instantiate<GameObject>(this.Piecepackpiece, SpawnPos, Quaternion.identity);
				gameObject122.GetComponent<MeshSyncScript>().SetMesh(11);
				return gameObject122;
			}
			if (ObjectName == "Bear")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgBEAR, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Chimera")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgCHIMERA, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Cyclops")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgCYCLOPS, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Dragonide")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgDRAGONIDE, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Evil Watcher")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgEVILWATCHER, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Giant Rat")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgRAT, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Giant Viper")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgGIANTVIPER, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Goblin")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgGOBLIN, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Golem")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgGOLEM, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Ghoul")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgGHOUL, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Griffon")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgGRIFFON, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Hydra")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgHYDRA, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Kobold")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgKOBOLD, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Lizard Warrior")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgLIZARDWARRIOR, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Manticora")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgMANTICORA, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Mummy")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgMUMMY, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Ogre")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgOGRE, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Orc")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgORC, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Skeleton Knight")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgSKELETONKNIGHT, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Tree Ent")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgTREEENT, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Troll")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgTROLL, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Werewolf")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgWEREWOLF, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Wolf")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgWOLF, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Wyvern")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgWYVERN, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Vampire")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgVAMPIRE, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Bear")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgBEAR, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Giant Rat")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgRAT, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Giant Viper")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgGIANTVIPER, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Wolf")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgWOLF, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Evil Watcher")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgEVILWATCHER, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Golem")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgGOLEM, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Tree Ent")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgTREEENT, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Dragonide")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgDRAGONIDE, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Hydra")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgHYDRA, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Lizard Warrior")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgLIZARDWARRIOR, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Wyvern")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgWYVERN, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Ghoul")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgGHOUL, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Mummy")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgMUMMY, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Skeleton Knight")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgSKELETONKNIGHT, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Vampire")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgVAMPIRE, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Cyclops")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgCYCLOPS, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Goblin")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgGOBLIN, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Kobold")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgKOBOLD, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Ogre")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgOGRE, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Orc")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgORC, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Troll")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgTROLL, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Chimera")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgCHIMERA, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Griffon")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgGRIFFON, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Manticora")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgMANTICORA, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Werewolf")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.rpgWEREWOLF, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Floor")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.TilesetFloor, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Wall")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.TilesetWall, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Corner")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.TilesetCorner, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Chest")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.TilesetChest, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Barrel")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.TilesetBarrel, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Table")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.TilesetTable, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Chair")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.TilesetChair, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Tree")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.TilesetTree, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Rock")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.TilesetRock, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Custom Figurine")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.CustomFigurine, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Knight of Knil")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.FigurineKnil, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Kimi Kat")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.FigurineKimiKats, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Sir Loin")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.FigurineLoin, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Mara")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.FigurineMara, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Zomblor")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.FigurineZomblor, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Zeke Kodoku")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.FigurineZeke, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "CardBot")
			{
				return UnityEngine.Object.Instantiate<GameObject>(this.FigurineCardBot, SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Backgammon Board")
			{
				string name9 = "backgammon_board";
				return UnityEngine.Object.Instantiate<GameObject>(this.GetPrefab(name9), SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Checkers Board")
			{
				string name10 = "Checker_Board";
				return UnityEngine.Object.Instantiate<GameObject>(this.GetPrefab(name10), SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Chess Board")
			{
				string name11 = "Chess_Board";
				return UnityEngine.Object.Instantiate<GameObject>(this.GetPrefab(name11), SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Chinese Checkers Board")
			{
				string name12 = "Chinese_Checkers_Board";
				return UnityEngine.Object.Instantiate<GameObject>(this.GetPrefab(name12), SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Custom Board")
			{
				string name13 = "Custom_Board";
				return UnityEngine.Object.Instantiate<GameObject>(this.GetPrefab(name13), SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Go Board")
			{
				string name14 = "Go_Board";
				return UnityEngine.Object.Instantiate<GameObject>(this.GetPrefab(name14), SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Pachisi Board")
			{
				string name15 = "Pachisi_board";
				return UnityEngine.Object.Instantiate<GameObject>(this.GetPrefab(name15), SpawnPos, Quaternion.identity);
			}
			if (ObjectName == "Reversi Board")
			{
				string name16 = "reversi_board";
				return UnityEngine.Object.Instantiate<GameObject>(this.GetPrefab(name16), SpawnPos, Quaternion.identity);
			}
			GameObject prefab2 = this.GetPrefab(ObjectName);
			if (prefab2)
			{
				return UnityEngine.Object.Instantiate<GameObject>(prefab2, SpawnPos, prefab2.transform.rotation);
			}
		}
		return null;
	}

	// Token: 0x04000941 RID: 2369
	[HideInInspector]
	public GameObject Piecepackpiece;

	// Token: 0x04000942 RID: 2370
	public GameObject TestObject;

	// Token: 0x04000943 RID: 2371
	private Quaternion YAXISFLIP = Quaternion.Euler(new Vector3(0f, -180f, 0f));

	// Token: 0x04000944 RID: 2372
	private List<int> IntList = new List<int>();

	// Token: 0x04000945 RID: 2373
	private int NumDominoes = 28;
}

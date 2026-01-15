using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using OpenTracksBETA.Resources;

namespace OpenTracksBETA.ViewModels
{
	// Token: 0x02000025 RID: 37
	public class MainViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001CD RID: 461 RVA: 0x0000C4F1 File Offset: 0x0000A6F1
		// (set) Token: 0x060001CE RID: 462 RVA: 0x0000C4F9 File Offset: 0x0000A6F9
		public ObservableCollection<TrackItem> Tracks { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000C502 File Offset: 0x0000A702
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000C50A File Offset: 0x0000A70A
		public ObservableCollection<TrackItem> NewTracks { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000C513 File Offset: 0x0000A713
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000C51B File Offset: 0x0000A71B
		public ObservableCollection<TrackItem> AllTracks { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000C524 File Offset: 0x0000A724
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x0000C52C File Offset: 0x0000A72C
		public ObservableCollection<TrackItem> AllPodcasts { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000C535 File Offset: 0x0000A735
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x0000C53D File Offset: 0x0000A73D
		public ObservableCollection<TrackItem> ChillTracks { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000C546 File Offset: 0x0000A746
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x0000C54E File Offset: 0x0000A74E
		public ObservableCollection<TrackItem> HypeTracks { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000C557 File Offset: 0x0000A757
		// (set) Token: 0x060001DA RID: 474 RVA: 0x0000C55F File Offset: 0x0000A75F
		public ObservableCollection<TrackItem> NATracks { get; set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000C568 File Offset: 0x0000A768
		// (set) Token: 0x060001DC RID: 476 RVA: 0x0000C570 File Offset: 0x0000A770
		public ObservableCollection<TrackItem> RTracks { get; set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000C579 File Offset: 0x0000A779
		// (set) Token: 0x060001DE RID: 478 RVA: 0x0000C581 File Offset: 0x0000A781
		public ObservableCollection<TrackItem> Favourites { get; set; }

		// Token: 0x060001DF RID: 479 RVA: 0x0000C58C File Offset: 0x0000A78C
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000C5B8 File Offset: 0x0000A7B8
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x0000C5D0 File Offset: 0x0000A7D0
		public ObservableCollection<TrackItem> FilteredTracks
		{
			get
			{
				return this._filteredTracks;
			}
			set
			{
				bool flag = this._filteredTracks != value;
				if (flag)
				{
					this._filteredTracks = value;
					this.OnPropertyChanged("FilteredTracks");
				}
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000C604 File Offset: 0x0000A804
		public MainViewModel()
		{
			ObservableCollection<TrackItem> observableCollection = new ObservableCollection<TrackItem>();
			observableCollection.Add(new TrackItem
			{
				Title = "Escalate",
				AudioPath = "https://ia902802.us.archive.org/27/items/escalate-1-osanime.com/Escalate1%20%28osanime.com%29.mp3",
				AlbumCoverPath = "https://dl8.wapkizfile.info/img/fcee952479b4f1596895ea12bf7cf89f/osanime+wapkizs+com/cover.jpg",
				Artist = "IRyS, Hololive"
			});
			observableCollection.Add(new TrackItem
			{
				Title = "Poster boy",
				AudioPath = "https://cs1.mp3.pm/download/241088001/TGlCay9pSE9kbG04WjdwMDJNbnFsK0hVSW8vNnNyMmNQUVdld2lzSW9Jc1EwWEdVK2ZzUWw3UWpybVc0REVXemxIa3FBZlF2SE9xVzNzQ0p5cDUyQ0ttWGZyU0ltQWtlRGRjMVVzUVRrZXg1Z3ArbWNvTTBKZlJWS3NhVlZFQ1U/2hollis_-_poster_boy_(mp3.pm).mp3",
				AlbumCoverPath = "https://images.genius.com/834c708cb90a7d4a704165032b74d371.500x500x1.png",
				Artist = "2hollis"
			});
			observableCollection.Add(new TrackItem
			{
				Title = "GET NO SATISFIED!",
				AudioPath = "https://fine.sunproxy.net/file/VHJHQS84RGtUT1N4djJJcUNCVHdVb3N0MkFFSGc2K3lKc0lKSDJSaG5TWGFWLzYyUmdJYU9nRmhMbHBOcG9HeUlyYmQ2Z0pHdm5CVGVQRHFwQWlaNG1ORGVvTWpHbGIvVXpOQVZtM3dGMm89/JAM_Project_feat.BABYMETAL_-_Get_No_Satisfied_One_Punch_Man_(SkySound.cc).mp3",
				Artist = "JAM PROJECT",
				AlbumCoverPath = "https://dl8.wapkizfile.info/img/56935d316e570b9f8fffa5ad45451e8e/osanime+wapkizs+com/cover.jpg"
			});
			observableCollection.Add(new TrackItem
			{
				Title = "вот как то так (Slowed)",
				AudioPath = "https://cs1.mp3.pm/download/237299536/TGlCay9pSE9kbG04WjdwMDJNbnFsK0hVSW8vNnNyMmNQUVdld2lzSW9Jdkx1MFlHRnB4Z2FwanBYb25FMWRnL1pSaE9yQThtRDNadVVFNXN3VVcvVGRlTEdRZ29GV1BCRnNiUE1ORVF4RG9HS0tJd1gzeU9icTJkUkFkUnVka2I/asanrap_-_vot_kak_to_tak_Slowed_(mp3.pm).mp3",
				AlbumCoverPath = "https://i.discogs.com/HykbYLCflnRo3rLf16BSA_LeSBp61jdT9spQWNJiRuQ/rs:fit/g:sm/q:90/h:468/w:450/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTE1Mzc2/NDAtMTMwMTIzNTY1/NS5qcGVn.jpeg",
				Artist = "asanrap"
			});
			observableCollection.Add(new TrackItem
			{
				Title = "Professional Griefers",
				AudioPath = "https://www.ostmusic.org/?view=file&format=raw&id=9316",
				AlbumCoverPath = "https://i1.sndcdn.com/artworks-000036361724-ugfe9i-t500x500.jpg",
				Artist = "deadmau5"
			});
			observableCollection.Add(new TrackItem
			{
				Title = "Bla bla bla",
				Artist = "di gigi d'agostino",
				AlbumCoverPath = "https://i.discogs.com/O8EaxlDVW-4UxoKR9Y8dcPKK-0jZesOaOWuFzyEYoNo/rs:fit/g:sm/q:90/h:575/w:465/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTcyMDMx/Mi0xMjU0OTU3NzQ5/LmpwZWc.jpeg",
				AudioPath = "https://fine.sunproxy.net/file/bXdLMjgyMWdsUEdpZXNuVDA4OE1iSTk3WlhxbU1Wd3pad3cvM2lXR2pTYkJnU0QrV2hoV2s5eGhqYkd6VlowN3o2UlFZa0tHOTY0akc3V3VTUStaOUVYYkp4OFVXa0tNQjVIK3JLc29zVkE9/Gigi_D_Agostino_-_Bla_Bla_Bla_(SkySound.cc).mp3"
			});
			observableCollection.Add(new TrackItem
			{
				Title = "Yummy (Righteous Remix, Clean VER.)",
				Artist = "AYESHA, itzzme",
				AlbumCoverPath = "https://source.boomplaymusic.com/group10/M00/04/02/dcda712b6a404df1aeba99762ac3e2b9_464_464.jpg",
				AudioPath = "https://cs1.mp3.pm/download/237297071/TGlCay9pSE9kbG04WjdwMDJNbnFsK0hVSW8vNnNyMmNQUVdld2lzSW9JdHA1Q3VVK1hHN0h1b3Rrc3prRExjUDN4TmlSM0Y0VElKUi9rdnZoQWFqUUVtUmV6M3JKUkI1VXZkRnBuUkRqdjRIeXZhWjJDQnIrMlFoVnBmSVl1c2g/Ayesha_Erotica_Mo_Beats_-_Yummy_-_Righteous_Remix_(mp3.pm).mp3"
			});
			observableCollection.Add(new TrackItem
			{
				Title = "Space Song",
				Artist = "BEACH HOUSE",
				AlbumCoverPath = "https://i1.sndcdn.com/artworks-R3WbcrvgiMze-0-t500x500.jpg",
				AudioPath = "https://ia804602.us.archive.org/19/items/y-2mate.com-beach-house-space-song/y2mate.com%20-%20Beach%20House%20%20Space%20Song.mp3"
			});
			observableCollection.Add(new TrackItem
			{
				Title = "Plastic Love (Night Tempo 100% Pure Remastered)",
				Artist = "Night Tempo, Takeuchi Mariya",
				AlbumCoverPath = "https://f4.bcbits.com/img/a3485050873_10.jpg",
				AudioPath = "https://ia803408.us.archive.org/2/items/soundcloud-652437512/Takeuchi%20Mariya%20-%20Plastic%20Love%20%28Night%20Tempo%20100%25%20Pure%20Remastered%29-652437512.mp3"
			});
			observableCollection.Add(new TrackItem
			{
				Title = "L'amour Toujours",
				Artist = "di gigi d'agostino",
				AlbumCoverPath = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQNHsuPIYrSX6xbCiQh-4-30onnrCy8YdkIvw&s",
				AudioPath = "https://cs1.mp3.pm/download/67121716/R0JaTDZSOGRGLzZxYU5DWXRkbGx1ZHBudVFvN3duQ0VzSVBoRm4rVmpVSTJwSzU3OVhpNG9sVlJvdzJFaVhoNEp4VkxkdHlURUxjOGJBU3M4bzlLYmNtbGZ2L0VxTFd0b1hCVGhta2JWUUREMk9kTGYyelM1VEhWTC83Qmp6UGU/Gigi_D_Agostino_-_L_amour_Toujours_Extended_(mp3.pm).mp3"
			});
			observableCollection.Add(new TrackItem
			{
				Title = "TIRED OF PROBLEMS",
				Artist = "NUEKI",
				AlbumCoverPath = "https://static.qobuz.com/images/covers/xb/hl/ecoy34x4nhlxb_600.jpg",
				AudioPath = "https://cs1.mp3.pm/download/238093516/TGlCay9pSE9kbG04WjdwMDJNbnFsK0hVSW8vNnNyMmNQUVdld2lzSW9Jc1NNNU02bG45U01RVEZIUzN2YTFZbUwxVVRkd2FXd1VSeEdmOXdCQ1lXZml4YmhDQ3JVeCtGaE52a2xqdCtlQlB0TXNKT21WektMVThnYjBKMjQrcE0/NUEKI_-_Tired_Of_Problems_Slowed_(mp3.pm).mp3"
			});
			observableCollection.Add(new TrackItem
			{
				Title = "Cold heart",
				Artist = "Dusky",
				AlbumCoverPath = "https://f4.bcbits.com/img/a2702590565_10.jpg",
				AudioPath = "https://cs1.mp3.pm/download/80399799/TGlCay9pSE9kbG04WjdwMDJNbnFsK0hVSW8vNnNyMmNQUVdld2lzSW9JdUwzeGhTc1Yrc1dnbXBBeHlaQjVxMHlMekduRnhBbkxIcGd2RU0wSTdBOU5xQVRjUFl0eDRUOVhVKy93UytNWFEvWlpSK2xsVGE1OXl3bUJseld3QVQ/Dusky_-_Cold_Heart_(mp3.pm).mp3"
			});
			this.Tracks = observableCollection;
			ObservableCollection<TrackItem> observableCollection2 = new ObservableCollection<TrackItem>();
			observableCollection2.Add(new TrackItem
			{
				Title = "GET NO SATISFIED! (25th S.P. LIVE VER)",
				AudioPath = "https://opsrv.vercel.app/chrisrlillo/JAM%20Project%20%E3%80%8CGet%20No%20Satisfied%20!%E3%80%8D25th%20Anniversary%20Live%20FINAL%20COUNTDOWN%20%5BOfficial%20Live%20Performance.mp3",
				Artist = "JAM PROJECT",
				AlbumCoverPath = "https://dl8.wapkizfile.info/img/56935d316e570b9f8fffa5ad45451e8e/osanime+wapkizs+com/cover.jpg"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Gloves 2 Ali",
				AudioPath = "https://opsrv.vercel.app/events/gloves2alinormal.mp3",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg",
				Artist = "RACEBACE"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "AE86 (Intro)",
				AudioPath = "https://opsrv.vercel.app/events/ae86.mp3",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg",
				Artist = "RACEBACE"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "My Limbo",
				AudioPath = "https://opsrv.vercel.app/events/mylimbo.mp3",
				Artist = "RACEBACE",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "The Way of a Man",
				AudioPath = "https://opsrv.vercel.app/events/wayofaman.mp3",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg",
				Artist = "RACEBACE"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "God's hand on a wheel",
				AudioPath = "https://opsrv.vercel.app/events/godshandonthewheel.mp3",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg",
				Artist = "RACEBACE"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Lost in Hell",
				AudioPath = "https://opsrv.vercel.app/events/lostinhell.mp3",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg",
				Artist = "RACEBACE"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Nevermind",
				AudioPath = "https://opsrv.vercel.app/events/nevermind.mp3",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg",
				Artist = "RACEBACE"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Champion!",
				AudioPath = "https://opsrv.vercel.app/events/champion.mp3",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg",
				Artist = "RACEBACE"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "First date~",
				AudioPath = "https://opsrv.vercel.app/events/firstdate.mp3",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg",
				Artist = "RACEBACE"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Lost the Good Things..",
				AudioPath = "https://opsrv.vercel.app/events/lostgoodthings.mp3",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg",
				Artist = "RACEBACE"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "A Racer's Dream",
				AudioPath = "https://opsrv.vercel.app/events/racerdream.mp3",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg",
				Artist = "RACEBACE"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Discovery of Akina Mountain",
				AudioPath = "https://opsrv.vercel.app/events/thediscoveryakinamountain.mp3",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg",
				Artist = "RACEBACE"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "We Roll!",
				AudioPath = "https://opsrv.vercel.app/events/weroll.mp3",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg",
				Artist = "RACEBACE"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Tanning in your sunray",
				AudioPath = "https://opsrv.vercel.app/events/tanninginyoursunray.mp3",
				AlbumCoverPath = "https://d3tvwjfge35btc.cloudfront.net/Assets/57/719/L_p1004071957.jpg",
				Artist = "RACEBACE"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "White Balcony",
				AudioPath = "https://opsrv.vercel.app/chrisrlillo/1%20-%20White%20Balcony.mp3",
				AlbumCoverPath = "https://opsrv.vercel.app/chrisrlillo/album.jpg",
				Artist = "ChrisRLillo"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Love & Hate",
				AudioPath = "https://opsrv.vercel.app/chrisrlillo/2%20-%20Love%20%26%20Hate.mp3",
				AlbumCoverPath = "https://opsrv.vercel.app/chrisrlillo/album.jpg",
				Artist = "ChrisRLillo"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Behind the line",
				AudioPath = "https://opsrv.vercel.app/chrisrlillo/3%20-%20Behind%20The%20Line.mp3",
				AlbumCoverPath = "https://opsrv.vercel.app/chrisrlillo/album.jpg",
				Artist = "ChrisRLillo"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Ghostly Touch",
				AudioPath = "https://opsrv.vercel.app/chrisrlillo/4%20-%20Ghostly%20Touch.mp3",
				AlbumCoverPath = "https://opsrv.vercel.app/chrisrlillo/album.jpg",
				Artist = "ChrisRLillo"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Decenso al Fuego",
				AudioPath = "https://opsrv.vercel.app/chrisrlillo/5%20-%20Decenso%20al%20Fuego.mp3",
				AlbumCoverPath = "https://opsrv.vercel.app/chrisrlillo/album.jpg",
				Artist = "ChrisRLillo"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Caos Y Miedo",
				AudioPath = "https://opsrv.vercel.app/chrisrlillo/6%20-%20Caos%20Y%20Miedo.mp3",
				AlbumCoverPath = "https://opsrv.vercel.app/chrisrlillo/album.jpg",
				Artist = "ChrisRLillo"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "T2K",
				AudioPath = "https://opsrv.vercel.app/chrisrlillo/7%20-%20T2k.mp3",
				AlbumCoverPath = "https://opsrv.vercel.app/chrisrlillo/album.jpg",
				Artist = "ChrisRLillo"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Por Una Noche Más",
				AudioPath = "https://opsrv.vercel.app/chrisrlillo/8%20-%20Por%20Una%20Noche%20M%C3%A1s.mp3",
				AlbumCoverPath = "https://opsrv.vercel.app/chrisrlillo/album.jpg",
				Artist = "ChrisRLillo"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Don't be scared",
				AudioPath = "https://opsrv.vercel.app/chrisrlillo/9%20-%20Don%27t%20Be%20Scared.mp3",
				AlbumCoverPath = "https://opsrv.vercel.app/chrisrlillo/album.jpg",
				Artist = "ChrisRLillo"
			});
			observableCollection2.Add(new TrackItem
			{
				Title = "Amor Fantasmal",
				AudioPath = "https://opsrv.vercel.app/chrisrlillo/10%20-%20Amor%20Fantasmal.mp3",
				AlbumCoverPath = "https://opsrv.vercel.app/chrisrlillo/album.jpg",
				Artist = "ChrisRLillo"
			});
			this.NewTracks = observableCollection2;
			ObservableCollection<TrackItem> observableCollection3 = new ObservableCollection<TrackItem>();
			observableCollection3.Add(new TrackItem
			{
				Title = "Bad Weekend",
				AudioPath = "https://ia800900.us.archive.org/25/items/allaround_202508/Asphalt%208_%20Airborne%20-%20Bad%20Weekend.mp3",
				Artist = "Krubb Wenkroist"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Rain",
				AudioPath = "https://ia800900.us.archive.org/25/items/allaround_202508/Initial%20D%205th%20Stage%20Soundtrack%20%20Rain.mp3",
				Artist = "Roa"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Bubble",
				AudioPath = "https://ia800900.us.archive.org/25/items/allaround_202508/Nightcore%20-%20Bubble.mp3",
				Artist = "Raven & Kreyn, Syrex"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "You Could be Mine",
				AudioPath = "https://ia800900.us.archive.org/25/items/allaround_202508/Dream%20Fighters%20-%20You%20Could%20Be%20Mine.mp3",
				Artist = "Dream Fighters"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Nekozilla",
				AudioPath = "https://ia803109.us.archive.org/27/items/generic-nekozilla-lfz-remix-_different-heaven_-6189037-wEqcGyYvm/nekozilla-lfz-remix-_different-heaven_-6189037-wEqcGyYvm-nekozilla-lfz-remix-_different-heaven_-6189037-wEqcGyYvm.mp3",
				Artist = "LFZ Remix"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Trinity",
				AudioPath = "https://ia801302.us.archive.org/2/items/009SoundSystemTrinityFullVersion/009%20Sound%20System%20-%20%21%20Trinity%20%28Full%20Version%29.mp3",
				AlbumCoverPath = "https://dl.marmak.net.pl/images/009%20Sound%20System/009%20Sound%20System.jpg",
				Artist = "009 sound system"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Yummy (Righteous Remix, Clean VER.)",
				Artist = "AYESHA, itzzme",
				AlbumCoverPath = "https://source.boomplaymusic.com/group10/M00/04/02/dcda712b6a404df1aeba99762ac3e2b9_464_464.jpg",
				AudioPath = "https://s3.ustatik.com/audio.com.audio/transcoding/00/62/1838769470886200-1838769471002828-1838769475125247.mp3?X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&X-Amz-Algorithm=AWS4-HMAC-SHA256&X-Amz-Credential=W7IA3NSYSOQIKLY9DEVC%2F20251124%2Feu-central-1%2Fs3%2Faws4_request&X-Amz-Date=20251124T155014Z&X-Amz-SignedHeaders=host&X-Amz-Expires=518400&X-Amz-Signature=ed8eb823d27771e32fb0831b01b1365ed39bb9b795c57e247ed0d2fb43287d3a"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Space Song",
				Artist = "BEACH HOUSE",
				AlbumCoverPath = "https://i1.sndcdn.com/artworks-R3WbcrvgiMze-0-t500x500.jpg",
				AudioPath = "https://ia804602.us.archive.org/19/items/y-2mate.com-beach-house-space-song/y2mate.com%20-%20Beach%20House%20%20Space%20Song.mp3"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "With a spirit",
				AudioPath = "https://ia600705.us.archive.org/24/items/009-sound-system-cd-rip/10%20-%20009%20Sound%20System%20-%20With%20A%20Spirit.mp3",
				AlbumCoverPath = "https://dl.marmak.net.pl/images/009%20Sound%20System/009%20Sound%20System.jpg",
				Artist = "009 sound system"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Escalate",
				AudioPath = "https://ia902802.us.archive.org/27/items/escalate-1-osanime.com/Escalate1%20%28osanime.com%29.mp3",
				AlbumCoverPath = "https://dl8.wapkizfile.info/img/fcee952479b4f1596895ea12bf7cf89f/osanime+wapkizs+com/cover.jpg",
				Artist = "IRyS, Hololive"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Poster boy",
				AudioPath = "https://cs1.mp3.pm/download/241088001/TGlCay9pSE9kbG04WjdwMDJNbnFsK0hVSW8vNnNyMmNQUVdld2lzSW9Jc1EwWEdVK2ZzUWw3UWpybVc0REVXemxIa3FBZlF2SE9xVzNzQ0p5cDUyQ0ttWGZyU0ltQWtlRGRjMVVzUVRrZXg1Z3ArbWNvTTBKZlJWS3NhVlZFQ1U/2hollis_-_poster_boy_(mp3.pm).mp3",
				AlbumCoverPath = "https://images.genius.com/834c708cb90a7d4a704165032b74d371.500x500x1.png",
				Artist = "2hollis"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Plastic Love (Night Tempo 100% Pure Remastered)",
				Artist = "Night Tempo, Takeuchi Mariya",
				AlbumCoverPath = "https://f4.bcbits.com/img/a3485050873_10.jpg",
				AudioPath = "https://ia803408.us.archive.org/2/items/soundcloud-652437512/Takeuchi%20Mariya%20-%20Plastic%20Love%20%28Night%20Tempo%20100%25%20Pure%20Remastered%29-652437512.mp3"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "L'amour Toujours",
				Artist = "di gigi d'agostino",
				AlbumCoverPath = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQNHsuPIYrSX6xbCiQh-4-30onnrCy8YdkIvw&s",
				AudioPath = "https://cs1.mp3.pm/download/67121716/R0JaTDZSOGRGLzZxYU5DWXRkbGx1ZHBudVFvN3duQ0VzSVBoRm4rVmpVSTJwSzU3OVhpNG9sVlJvdzJFaVhoNEp4VkxkdHlURUxjOGJBU3M4bzlLYmNtbGZ2L0VxTFd0b1hCVGhta2JWUUREMk9kTGYyelM1VEhWTC83Qmp6UGU/Gigi_D_Agostino_-_L_amour_Toujours_Extended_(mp3.pm).mp3"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "TIRED OF PROBLEMS",
				Artist = "NUEKI",
				AlbumCoverPath = "https://static.qobuz.com/images/covers/xb/hl/ecoy34x4nhlxb_600.jpg",
				AudioPath = "https://cs1.mp3.pm/download/238093516/TGlCay9pSE9kbG04WjdwMDJNbnFsK0hVSW8vNnNyMmNQUVdld2lzSW9Jc1NNNU02bG45U01RVEZIUzN2YTFZbUwxVVRkd2FXd1VSeEdmOXdCQ1lXZml4YmhDQ3JVeCtGaE52a2xqdCtlQlB0TXNKT21WektMVThnYjBKMjQrcE0/NUEKI_-_Tired_Of_Problems_Slowed_(mp3.pm).mp3"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Cold heart",
				Artist = "Dusky",
				AlbumCoverPath = "https://f4.bcbits.com/img/a2702590565_10.jpg",
				AudioPath = "https://cs1.mp3.pm/download/80399799/TGlCay9pSE9kbG04WjdwMDJNbnFsK0hVSW8vNnNyMmNQUVdld2lzSW9JdUwzeGhTc1Yrc1dnbXBBeHlaQjVxMHlMekduRnhBbkxIcGd2RU0wSTdBOU5xQVRjUFl0eDRUOVhVKy93UytNWFEvWlpSK2xsVGE1OXl3bUJseld3QVQ/Dusky_-_Cold_Heart_(mp3.pm).mp3"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Miracle Rose",
				AudioPath = "https://ia601000.us.archive.org/2/items/eva_20250816_202508/miraclerose.mp3",
				AlbumCoverPath = "https://static.zerochan.net/Initial.D.full.1184790.jpg",
				Artist = "Galla, いさみんチャンネル"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "You",
				AudioPath = "https://ia804607.us.archive.org/31/items/ncsyt-13-kontinuum-lost-feat.-savoi-jjd-remix/NIVIRO%20-%20You%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://blogger.googleusercontent.com/img/b/R29vZ2xl/AVvXsEg6Apzv91t3j7f9FqJdrAnzXxAocDgI8E4IuWnpyPSBO7k5q837Ov2GWHxBTrhoLjU_Skp-xFGEyky2yuwNMsA_7Ht2GxjSZVg4O5D_Kp6d91LvVB_eQOn21LbnkGnJsA-5kftYf1D1uatc/w1200-h630-p-k-no-nu/street_fighter_ex3_cover.jpg",
				Artist = "NIVIRO"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Save Another day for me",
				AudioPath = "https://ia902302.us.archive.org/35/items/de-leo-save-another-day-for-me/De%20Leo%20-%20Save%20Another%20Day%20For%20Me.mp3",
				AlbumCoverPath = "https://i.discogs.com/fFGJBrZI_jyGAdrprDaThtBt0gjecrn2Tgt9RtHiFME/rs:fit/g:sm/q:90/h:400/w:397/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTE0MTUw/MjEzLTE1Njg3NjMw/NDMtNDczMC5qcGVn.jpeg",
				Artist = "De Leo"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Still I love you (NC)",
				AudioPath = "https://ia600905.us.archive.org/21/items/nightcoremixes10243/stilliloveyou.mp3",
				AlbumCoverPath = "https://static.wikia.nocookie.net/school-days/images/d/dc/Cover.jpg/revision/latest?cb=20200511110231",
				Artist = "JPREMIXI"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "イノセント・ブルー (NC)",
				AudioPath = "https://ia803203.us.archive.org/33/items/eva_20250816/pureblue.mp3",
				AlbumCoverPath = "https://static.wikia.nocookie.net/school-days/images/d/dc/Cover.jpg/revision/latest?cb=20200511110231",
				Artist = "JP REMIXI"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "紅蓮華 EUROBEAT Remix",
				AudioPath = "https://ia903203.us.archive.org/33/items/eva_20250816/gurenge.mp3",
				AlbumCoverPath = "https://i1.sndcdn.com/artworks-000587244338-vn5orc-t500x500.jpg",
				Artist = "Turbo"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "A Cruel Angels Thesis Eurobeat Remix",
				AudioPath = "https://ia803203.us.archive.org/33/items/eva_20250816/eva.mp3",
				AlbumCoverPath = "https://i1.sndcdn.com/artworks-000567230594-q8qfvo-t1080x1080.jpg",
				Artist = "Turbo"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "立ち上がリーヨ!",
				AudioPath = "https://ia803203.us.archive.org/33/items/eva_20250816/%E7%AB%8B%E3%81%A1%E4%B8%8A%E3%81%8C%E3%83%AA%E3%83%BC%E3%83%A8.mp3",
				AlbumCoverPath = "https://cdn.aniplaylist.com/thumbnails/aE93evFQvpEFc1CeKHTXdgvVrKnPLEoJdizRE2MJ.jpeg",
				Artist = "T-Pistonz, pugcat"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Mes pensées le soir (V1)",
				AudioPath = "https://ia600906.us.archive.org/29/items/penseesdeauville/penseesdeauville.mp3",
				AlbumCoverPath = "https://source.boomplaymusic.com/group10/M00/06/29/ffeab3b4a91f46daa89f504b1958261d_320_320.jpg",
				Artist = "Smoking Deauville"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "GET NO SATISFIED!",
				AudioPath = "https://fine.sunproxy.net/file/VHJHQS84RGtUT1N4djJJcUNCVHdVb3N0MkFFSGc2K3lKc0lKSDJSaG5TWGFWLzYyUmdJYU9nRmhMbHBOcG9HeUlyYmQ2Z0pHdm5CVGVQRHFwQWlaNG1ORGVvTWpHbGIvVXpOQVZtM3dGMm89/JAM_Project_feat.BABYMETAL_-_Get_No_Satisfied_One_Punch_Man_(SkySound.cc).mp3",
				Artist = "JAM PROJECT",
				AlbumCoverPath = "https://dl8.wapkizfile.info/img/56935d316e570b9f8fffa5ad45451e8e/osanime+wapkizs+com/cover.jpg"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "вот как то так (Slowed)",
				AudioPath = "https://cs1.mp3.pm/download/237299536/TGlCay9pSE9kbG04WjdwMDJNbnFsK0hVSW8vNnNyMmNQUVdld2lzSW9Jdkx1MFlHRnB4Z2FwanBYb25FMWRnL1pSaE9yQThtRDNadVVFNXN3VVcvVGRlTEdRZ29GV1BCRnNiUE1ORVF4RG9HS0tJd1gzeU9icTJkUkFkUnVka2I/asanrap_-_vot_kak_to_tak_Slowed_(mp3.pm).mp3",
				AlbumCoverPath = "https://i.discogs.com/HykbYLCflnRo3rLf16BSA_LeSBp61jdT9spQWNJiRuQ/rs:fit/g:sm/q:90/h:468/w:450/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTE1Mzc2/NDAtMTMwMTIzNTY1/NS5qcGVn.jpeg",
				Artist = "asanrap"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Professional Griefers",
				AudioPath = "https://www.ostmusic.org/?view=file&format=raw&id=9316",
				AlbumCoverPath = "https://i1.sndcdn.com/artworks-000036361724-ugfe9i-t500x500.jpg",
				Artist = "deadmau5"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Bla bla bla",
				Artist = "di gigi d'agostino",
				AlbumCoverPath = "https://i.discogs.com/O8EaxlDVW-4UxoKR9Y8dcPKK-0jZesOaOWuFzyEYoNo/rs:fit/g:sm/q:90/h:575/w:465/czM6Ly9kaXNjb2dz/LWRhdGFiYXNlLWlt/YWdlcy9SLTcyMDMx/Mi0xMjU0OTU3NzQ5/LmpwZWc.jpeg",
				AudioPath = "https://fine.sunproxy.net/file/bXdLMjgyMWdsUEdpZXNuVDA4OE1iSTk3WlhxbU1Wd3pad3cvM2lXR2pTYkJnU0QrV2hoV2s5eGhqYkd6VlowN3o2UlFZa0tHOTY0akc3V3VTUStaOUVYYkp4OFVXa0tNQjVIK3JLc29zVkE9/Gigi_D_Agostino_-_Bla_Bla_Bla_(SkySound.cc).mp3"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "caramelldansen (nightcore)",
				AudioPath = "https://ia801000.us.archive.org/2/items/eva_20250816_202508/caramelldansen.mp3",
				AlbumCoverPath = "https://i.kfs.io/album/global/68318497,0v1/fit/500x500.jpg",
				Artist = "Caramella Girls, KidousRemix"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Windows",
				AudioPath = "https://ia801701.us.archive.org/11/items/stilliloveyou/K-391%20-%20Windows.mp3",
				Artist = "K391"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "STARVING RAVEN",
				AudioPath = "https://ia800906.us.archive.org/29/items/penseesdeauville/%E3%80%90%E6%9D%B1%E6%96%B9Vocal%EF%BC%8FEurobeat%E3%80%91%20STARVING%20RAVEN%20%E3%80%8CSOUND%20HOLIC%E3%80%8D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/2.jpg",
				Artist = "nana takahashi, jp remixi"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "NIGHTS OF NIGHTS",
				AudioPath = "https://ia601500.us.archive.org/12/items/all-night-of-knights-cool-and-create/01.%20Night%20of%20Knights.mp3",
				AlbumCoverPath = "https://dl.marmak.net.pl/images/009%20Sound%20System/009%20Sound%20System.jpg",
				Artist = "Flowering Night Remix"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "IGNITE THE POWER",
				AudioPath = "https://ia800906.us.archive.org/29/items/penseesdeauville/IGNITE%20THE%20POWER.mp3",
				AlbumCoverPath = "https://i1.sndcdn.com/artworks-000587244338-vn5orc-t500x500.jpg",
				Artist = "nana takahashi, th remix"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "00 HEAVEN",
				AudioPath = "https://ia800906.us.archive.org/29/items/penseesdeauville/%E3%80%90%E6%9D%B1%E6%96%B9Vocal%EF%BC%8FEurobeat%E3%80%91%2000%20HEAVEN%20%E3%80%8CSOUND%20HOLIC%E3%80%8D.mp3",
				AlbumCoverPath = "https://i1.sndcdn.com/artworks-000567230594-q8qfvo-t1080x1080.jpg",
				Artist = "nana takahashi, th remix"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "FIRST STORM",
				AudioPath = "https://ia800906.us.archive.org/29/items/penseesdeauville/FIRST%20STORM%20%EF%BD%9C%20%F0%9D%90%BB%F0%9D%91%8E%F0%9D%91%A1%F0%9D%91%A0%F0%9D%91%A2%F0%9D%91%9B%F0%9D%91%92%20%F0%9D%91%80%F0%9D%91%96%F0%9D%91%98%F0%9D%91%A2%20%EF%BD%9C%20No%20Copyright%20%F0%9F%8E%B5.mp3",
				AlbumCoverPath = "https://dl.marmak.net.pl/images/009%20Sound%20System/009%20Sound%20System.jpg",
				Artist = "Hatsune Miku"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "HAZY MOON",
				AudioPath = "https://ia600906.us.archive.org/29/items/penseesdeauville/Hazy%20Moon%20-%20Hatsune%20Miku%20%28%20no%20copyright%20music%20%29.mp3",
				AlbumCoverPath = "https://i1.sndcdn.com/artworks-000587244338-vn5orc-t500x500.jpg",
				Artist = "Hatsune Miku"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Ievan Polkka (Sped up)",
				AudioPath = "https://ia800906.us.archive.org/29/items/penseesdeauville/Ievan%20Polkka%20Song%20-%20Hatsune%20Miku%20%28Copyright%20Free%20Song%29.mp3",
				AlbumCoverPath = "https://i1.sndcdn.com/artworks-000567230594-q8qfvo-t1080x1080.jpg",
				Artist = "Hatsune Miku"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "DIVE INTO STREAM",
				AudioPath = "https://ia600906.us.archive.org/29/items/penseesdeauville/dintos.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/2.jpg",
				Artist = "Hatsune Miku"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Adventure",
				AudioPath = "https://ia600900.us.archive.org/25/items/allaround_202508/Adventure.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/3.jpg",
				Artist = "JJD"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Arrow",
				AudioPath = "https://ia800900.us.archive.org/25/items/allaround_202508/Arrow.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/1.jpg",
				Artist = "Jim Yosef"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Blank",
				AudioPath = "https://ia903209.us.archive.org/18/items/DisfigureBlankNCSRelease/Disfigure%20-%20Blank%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/2.jpg",
				Artist = "Disfigure"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Candyland",
				AudioPath = "https://ia903205.us.archive.org/31/items/TobuCandylandNCSRelease/Tobu%20-%20Candyland%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/4.jpeg",
				Artist = "Tobu"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Cetus",
				AudioPath = "https://ia601902.us.archive.org/10/items/LenskoCetusNCSRelease/Lensko%20-%20Cetus%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/5.jpg",
				Artist = "Lensko"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Circles",
				AudioPath = "https://ia903206.us.archive.org/7/items/LenskoCirclesNCSRelease/Lensko%20-%20Circles%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/5.jpg",
				Artist = "Lensko"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Seven",
				AudioPath = "https://ia801904.us.archive.org/8/items/TobuSevenNCSRelease/Tobu%20-%20Seven%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/1.jpg",
				Artist = "Tobu"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Energy",
				AudioPath = "https://ia601904.us.archive.org/4/items/ElektronomiaEnergyNCSRelease/Elektronomia%20-%20Energy%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/5.jpg",
				Artist = "Elektronomia"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Entropy",
				AudioPath = "https://ia803203.us.archive.org/34/items/DistrionAlexSkrindoEntropyNCSRelease/Distrion%20%20Alex%20Skrindo%20-%20Entropy%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/5.jpg",
				Artist = "Distrion, Alex"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Good Times",
				AudioPath = "https://ia801708.us.archive.org/1/items/soundcloud-817542118/817542118.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/1.jpg",
				Artist = "Tobu"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Hellcat",
				AudioPath = "https://ia801903.us.archive.org/30/items/DesmeonHellcatNCSRelease/Desmeon%20-%20Hellcat%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/3.jpg",
				Artist = "Desmon"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Hello",
				AudioPath = "https://ia801209.us.archive.org/1/items/OMFGHello_20150908/OMFG%20-%20Hello.mp3",
				AlbumCoverPath = "https://i1.sndcdn.com/artworks-000101331401-iecrgv-t500x500.jpg",
				Artist = "OMFG"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "High",
				AudioPath = "https://ia801902.us.archive.org/26/items/JPBHighNCSRelease/JPB%20-%20High%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/2.jpg",
				Artist = "JPB"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Higher",
				AudioPath = "https://ia800306.us.archive.org/17/items/TobuHigher/Tobu%20-%20Higher.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/2.jpg",
				Artist = "Tobu"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Hope",
				AudioPath = "https://ia803207.us.archive.org/32/items/TobuHopeNCSRelease_201612/Tobu%20-%20Hope%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/2.jpg",
				Artist = "Tobu"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Limitless",
				AudioPath = "https://ia804607.us.archive.org/31/items/ncsyt-13-kontinuum-lost-feat.-savoi-jjd-remix/Elektronomia%20-%20Limitless%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/5.jpg",
				Artist = "Elektronomia"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Monody",
				AudioPath = "https://ia801304.us.archive.org/31/items/gdps-2.2-song-652927/652927.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/2.jpg",
				Artist = "TheFatRat"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Puzzle",
				AudioPath = "https://ia801702.us.archive.org/8/items/soundcloud-317698312/317698312.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/4.jpeg",
				Artist = "RetroVision"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Windfall",
				AudioPath = "https://ia801203.us.archive.org/21/items/gdps-2.2-song-621135/621135.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/1.jpg",
				Artist = "TheFatRat"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Xenogenesis",
				AudioPath = "https://ia800506.us.archive.org/12/items/gdps-2.2-song-621136/621136.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/5.jpg",
				Artist = "TheFatRat"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Let's Go!",
				AudioPath = "https://ia803207.us.archive.org/30/items/LenskoLetsGoNCSRelease/Lensko%20-%20Lets%20Go%21%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/5.jpg",
				Artist = "Lensko"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Nekozilla",
				AudioPath = "https://ia803207.us.archive.org/20/items/whats212139all/Nekozilla.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/2.jpg",
				Artist = "Different Heaven"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Ahrix - Nova",
				AudioPath = "https://ia803201.us.archive.org/1/items/AhrixNovaNCSRelease_201612/Ahrix%20-%20Nova%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Homework%20Assets/n.jpg",
				Artist = "Ahrix"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Popsicle",
				AudioPath = "https://ia902900.us.archive.org/30/items/LFZPopsicleOriginalMix/LFZ%20-%20Popsicle%20%28Original%20Mix%29.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/5.jpg",
				Artist = "LFZ"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Sky High",
				AudioPath = "https://ia804607.us.archive.org/31/items/ncsyt-13-kontinuum-lost-feat.-savoi-jjd-remix/Elektronomia%20-%20Sky%20High%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/5.jpg",
				Artist = "Elektronomia"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Sun burst",
				AudioPath = "https://ia903205.us.archive.org/35/items/TobuItroSunburstNCSRelease/Tobu%20%20Itro%20-%20Sunburst%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Homework%20Assets/t.jpg",
				Artist = "Tobu"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Symbolism",
				AudioPath = "https://ia801705.us.archive.org/24/items/electro-light-symbolism-ncs-release/Electro-Light%20-%20Symbolism%20%5BNCS%20Release%5D.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/ncs/5.jpg",
				Artist = "Electro-Light"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Unity",
				AudioPath = "https://ia801205.us.archive.org/28/items/gdps-2.2-song-621134/621134.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Homework%20Assets/ft.jpg",
				Artist = "TheFatRat"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Fluxxwave (REMIX)",
				AudioPath = "http://droidjacks4.helioho.st/fluxrm.mp3",
				AlbumCoverPath = "https://i.scdn.co/image/ab67616d0000b273aa00c31473fe4542a1ed81a2",
				Artist = "clovis reyes, EPROVES REMIX"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "WAKEUP (REMIX)",
				AudioPath = "https://ia801000.us.archive.org/2/items/eva_20250816_202508/wakeup1.mp3",
				AlbumCoverPath = "https://i.scdn.co/image/ab67616d0000b273a3a0d9665cc88b29b1d69f8f",
				Artist = "MOONDEITY, EPROVES REMIX"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "IMMACULATE (REMIX)",
				AudioPath = "https://ia801000.us.archive.org/2/items/eva_20250816_202508/immaculateremix.mp3",
				AlbumCoverPath = "https://i.scdn.co/image/ab67616d0000b273a3a0d9665cc88b29b1d69f8f",
				Artist = "ASPHALT REMIX"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Maybe Tonight Remix",
				AudioPath = "https://ia801000.us.archive.org/2/items/eva_20250816_202508/maybetonight%20%282%29.mp3",
				AlbumCoverPath = "https://i1.sndcdn.com/artworks-000123477049-b0o5t1-t500x500.jpg",
				Artist = "MAKO & SAYUKI"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Lunar Abyss",
				AudioPath = "https://ia801304.us.archive.org/19/items/gdps-2.2-song-645631/645631.mp3",
				AlbumCoverPath = "https://i.scdn.co/image/ab67616d0000b273f0d6546794cb5d0b5d41588b",
				Artist = "Lchavasse"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "All Around",
				AudioPath = "https://ia600900.us.archive.org/25/items/allaround_202508/allaround.mp3",
				AlbumCoverPath = "https://static.wikia.nocookie.net/eurobeat/images/6/63/EYCA-12185-6_Cover.jpg/revision/latest/scale-to-width/360?cb=20220314234118",
				Artist = "Lia"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Secret Love",
				AudioPath = "https://finaldistance.net/mp3/Initial%20D%20Fourth%20Stage%20D%20Selection%20%2B/Nutty%20-%20Secret%20Love.mp3",
				AlbumCoverPath = "https://static.wikia.nocookie.net/eurobeat/images/4/4c/Initial_D_Vocal_Battle_Special_feat_Takahashi_Bros_Red_Suns.jpg/revision/latest?cb=20210515170210",
				Artist = "Nutty"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "Don't Go Baby",
				AudioPath = "https://finaldistance.net/mp3/Initial%20D%20Fourth%20Stage%20D%20Selection%20%2B/Maiko%20-%20Don%60t%20Go%20Baby.mp3",
				AlbumCoverPath = "https://static.wikia.nocookie.net/eurobeat/images/4/4c/Initial_D_Vocal_Battle_Special_feat_Takahashi_Bros_Red_Suns.jpg/revision/latest?cb=20210515170210",
				Artist = "Maiko"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "CATATONIC",
				AudioPath = "https://ia601701.us.archive.org/11/items/stilliloveyou/CATATONIC%20-%20fatestchan.mp3",
				AlbumCoverPath = "https://i.scdn.co/image/ab67616d0000b273aa00c31473fe4542a1ed81a2",
				Artist = "fatestchan"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "ROCKIN' Hardcore",
				AudioPath = "https://ia804506.us.archive.org/1/items/initial-d-5th-stage-soundtrack-rockin-hardcore-reupload_202107/Initial%20D%205th%20Stage%20Soundtrack%20-%20Rockin%27%20Hardcore%20%28REUPLOAD%29.mp3",
				Artist = "Fastway"
			});
			observableCollection3.Add(new TrackItem
			{
				Title = "PROJECT III Remix",
				AudioPath = "https://ia600906.us.archive.org/29/items/penseesdeauville/Initial%20D%205th%20Stage%20Sound%20File%20-%20Project%20D%20III.mp3",
				AlbumCoverPath = "https://i.scdn.co/image/ab67616d0000b273f0d6546794cb5d0b5d41588b",
				Artist = "ARYHM Remix"
			});
			this.AllTracks = observableCollection3;
			this.FilteredTracks = new ObservableCollection<TrackItem>(this.AllTracks);
			ObservableCollection<TrackItem> observableCollection4 = new ObservableCollection<TrackItem>();
			observableCollection4.Add(new TrackItem
			{
				Title = "午夜白卡談 - 和平&健康",
				AudioPath = "https://opsrv.vercel.app/narrowpdct.mp3",
				AlbumCoverPath = "https://opsrv.vercel.app/bakatalk.png",
				Artist = "午夜白卡談"
			});
			observableCollection4.Add(new TrackItem
			{
				Title = "b1an - Win8 in 2024? worth it?",
				AudioPath = "https://open-tracks.vercel.app/subpage/podcasts/chockingnetdude/tutor1-2.mp3",
				AlbumCoverPath = "https://open-tracks.vercel.app/Assets/test.jpg",
				Artist = "b1an"
			});
			observableCollection4.Add(new TrackItem
			{
				Title = "ChockingNetDude - Tutorial",
				AudioPath = "https://opsrv.vercel.app/tutu.mp3",
				Artist = "ChockingNetDude"
			});
			this.AllPodcasts = observableCollection4;
			ObservableCollection<TrackItem> observableCollection5 = new ObservableCollection<TrackItem>();
			observableCollection5.Add(new TrackItem
			{
				Title = "1 hr nonstop - Super Lofi ",
				AudioPath = "https://opsrv.vercel.app/lofi.mp3"
			});
			observableCollection5.Add(new TrackItem
			{
				Title = "1 hr nonstop - Stream Cafe ",
				AudioPath = "https://opsrv.vercel.app/lofi1.mp3"
			});
			observableCollection5.Add(new TrackItem
			{
				Title = "30 mins nonstop - Count Lofi ",
				AudioPath = "https://opsrv.vercel.app/lofi2.mp3"
			});
			observableCollection5.Add(new TrackItem
			{
				Title = "30 mins nonstop - SOUL LOFI ",
				AudioPath = "https://opsrv.vercel.app/lofi3.mp3"
			});
			observableCollection5.Add(new TrackItem
			{
				Title = "SmokeDeau - Mes pensées le soir (V1)",
				AudioPath = "https://opsrv.vercel.app/penseesdeauville.mp3"
			});
			this.ChillTracks = observableCollection5;
			ObservableCollection<TrackItem> observableCollection6 = new ObservableCollection<TrackItem>();
			observableCollection6.Add(new TrackItem
			{
				Title = "MOONDEITY - WAKEUP (EPROVES REMIX)",
				AudioPath = "https://opsrv.vercel.app/wakeup.mp3"
			});
			observableCollection6.Add(new TrackItem
			{
				Title = "Lchavasse - Lunar Abyss",
				AudioPath = "https://opsrv.vercel.app/lunarabyss.mp3"
			});
			observableCollection6.Add(new TrackItem
			{
				Title = "Akina Mountain - SPD Remix",
				AudioPath = "https://opsrv.vercel.app/headpac.mp3"
			});
			observableCollection6.Add(new TrackItem
			{
				Title = "Preserved Vampire - Dimention Movie Remix",
				AudioPath = "https://opsrv.vercel.app/pvam.mp3"
			});
			observableCollection6.Add(new TrackItem
			{
				Title = "No Life Queen - DJ Command Remix",
				AudioPath = "https://opsrv.vercel.app/nlq.mp3"
			});
			observableCollection6.Add(new TrackItem
			{
				Title = "Sugar & Spice - DJ Rexy Remix",
				AudioPath = "https://opsrv.vercel.app/sugarspice.mp3"
			});
			observableCollection6.Add(new TrackItem
			{
				Title = "DeLeo - Save Another day for me",
				AudioPath = "https://opsrv.vercel.app/saveanotherdayforme.mp3"
			});
			observableCollection6.Add(new TrackItem
			{
				Title = "fate$tchan - Betrayal LV.2",
				AudioPath = "https://opsrv.vercel.app/betrayal2.mp3"
			});
			observableCollection6.Add(new TrackItem
			{
				Title = "Battle Formula - Sudo Kyoichii",
				AudioPath = "https://opsrv.vercel.app/sudo.mp3"
			});
			observableCollection6.Add(new TrackItem
			{
				Title = "clovis reyes - Fluxxwave (EPROVES REMIX)",
				AudioPath = "https://opsrv.vercel.app/fluxrm.mp3"
			});
			observableCollection6.Add(new TrackItem
			{
				Title = "Precious Heart - SSG",
				AudioPath = "https://opsrv.vercel.app/preciousheart.mp3"
			});
			this.HypeTracks = observableCollection6;
			ObservableCollection<TrackItem> observableCollection7 = new ObservableCollection<TrackItem>();
			observableCollection7.Add(new TrackItem
			{
				Title = "fate$tchan - Betrayal LV.2",
				AudioPath = "https://opsrv.vercel.app/betrayal2.mp3"
			});
			observableCollection7.Add(new TrackItem
			{
				Title = "Land of future - Demoner latinus",
				AudioPath = "https://opsrv.vercel.app/landofuture.mp3"
			});
			observableCollection7.Add(new TrackItem
			{
				Title = "fate$tchan - betrayal LV.1",
				AudioPath = "https://opsrv.vercel.app/betrayal1.mp3"
			});
			this.NATracks = observableCollection7;
			ObservableCollection<TrackItem> observableCollection8 = new ObservableCollection<TrackItem>();
			observableCollection8.Add(new TrackItem
			{
				Title = "Maybe Tonight - Mako&Sayuki",
				AudioPath = "https://opsrv.vercel.app/maybetonight.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "Takahashi Bros - Miracle Rose",
				AudioPath = "https://opsrv.vercel.app/miraclerose.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "Feel it - SPD Remix",
				AudioPath = "https://opsrv.vercel.app/feelit.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "Akina Mountain - SPD Remix",
				AudioPath = "https://opsrv.vercel.app/headpac.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "MOONDEITY - WAKEUP (EPROVES REMIX)",
				AudioPath = "https://opsrv.vercel.app/betrayal2o.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "clovis reyes - Fluxxwave (EPROVES REMIX)",
				AudioPath = "https://opsrv.vercel.app/fluxrm.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "fate$tchan - betrayal LV.2 (JP REMIX)",
				AudioPath = "https://opsrv.vercel.app/betrayal2o.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "rgLed - Win7 Error Dubstep Remix",
				AudioPath = "https://opsrv.vercel.app/w7er.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "rgLed - Win8 Error Dubstep Remix",
				AudioPath = "https://opsrv.vercel.app/w8er.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "Crowd Pleasers - SPD remix",
				AudioPath = "https://opsrv.vercel.app/vol1.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "Sugar & Spice - DJ Rexy Remix",
				AudioPath = "https://opsrv.vercel.app/sugarspice.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "Rainbow in the sky - SPD Remix",
				AudioPath = "https://opsrv.vercel.app/rainbow.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "Akina Mountain - SPD Remix",
				AudioPath = "https://opsrv.vercel.app/headpac.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "Preserved Vampire - Dimention Movie Remix",
				AudioPath = "https://opsrv.vercel.app/pvam.mp3"
			});
			observableCollection8.Add(new TrackItem
			{
				Title = "No Life Queen - DJ Command Remix",
				AudioPath = "https://opsrv.vercel.app/nlq.mp3"
			});
			this.RTracks = observableCollection8;
			this.Favourites = new ObservableCollection<TrackItem>();
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000E678 File Offset: 0x0000C878
		public void FilterTracks(string query)
		{
			bool flag = string.IsNullOrWhiteSpace(query);
			if (flag)
			{
				this.FilteredTracks = new ObservableCollection<TrackItem>(this.AllTracks);
			}
			else
			{
				List<TrackItem> list = new List<TrackItem>();
				foreach (TrackItem trackItem in this.AllTracks)
				{
					bool flag2 = trackItem.Title != null && trackItem.Title.ToLower().Contains(query.ToLower());
					if (flag2)
					{
						list.Add(trackItem);
					}
				}
				this.FilteredTracks = new ObservableCollection<TrackItem>(list);
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000E72C File Offset: 0x0000C92C
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x0000E744 File Offset: 0x0000C944
		public string SampleProperty
		{
			get
			{
				return this._sampleProperty;
			}
			set
			{
				bool flag = value != this._sampleProperty;
				if (flag)
				{
					this._sampleProperty = value;
					this.NotifyPropertyChanged("SampleProperty");
				}
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000E778 File Offset: 0x0000C978
		public string LocalizedSampleProperty
		{
			get
			{
				return AppResources.SampleProperty;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x0000E78F File Offset: 0x0000C98F
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x0000E797 File Offset: 0x0000C997
		public bool IsDataLoaded { get; private set; }

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060001E9 RID: 489 RVA: 0x0000E7A0 File Offset: 0x0000C9A0
		// (remove) Token: 0x060001EA RID: 490 RVA: 0x0000E7D8 File Offset: 0x0000C9D8
		[DebuggerBrowsable(0)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x060001EB RID: 491 RVA: 0x0000E810 File Offset: 0x0000CA10
		private void NotifyPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			bool flag = propertyChanged != null;
			if (flag)
			{
				propertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x04000124 RID: 292
		public ObservableCollection<TrackItem> currentTrackList;

		// Token: 0x0400012C RID: 300
		private ObservableCollection<TrackItem> _filteredTracks;

		// Token: 0x0400012D RID: 301
		private string _sampleProperty = "Sample Runtime Property Value";

		// Token: 0x0200006E RID: 110
		public class PlayAudioMessage
		{
			// Token: 0x1700008F RID: 143
			// (get) Token: 0x06000307 RID: 775 RVA: 0x00013388 File Offset: 0x00011588
			// (set) Token: 0x06000308 RID: 776 RVA: 0x00013390 File Offset: 0x00011590
			public string Path { get; set; }
		}
	}
}

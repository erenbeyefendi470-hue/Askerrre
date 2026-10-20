PC'SIZ (SADECE TELEFONLA) APK ALMA REHBERI
===========================================
Fikir: Proje GitHub'a yuklenir, GitHub'in sunucusu (GitHub Actions +
GameCI) Unity'yi orada calistirip APK uretir, sen APK'yi telefona
indirirsin. PC gerekmez ama telefonla biraz ugrastirir. Sabir + Wi-Fi sart.

KIMSEYE VERME: Unity sifreni, GitHub tokenini, lisans dosyanin icerigini
bu sohbete YAZMA. Hepsini sen kendi telefonunda ilgili sitelere girersin.

HAZIRLIK (bu projede zaten var)
-------------------------------
.github/workflows/activation.yml   -> Unity lisans dosyasi isteme isi
.github/workflows/build-android.yml -> APK build isi (Unity 2019.4.15f1)
Assets/Editor/BuildAndroid.cs      -> build metodu (BuildAndroid.BuildForCI)

ADIM 1 - Hesaplar (telefondan)
------------------------------
1) github.com -> hesap ac.
2) unity.com / Unity ID -> hesap ac (ucretsiz Personal icin).

ADIM 2 - Bos repo ac
--------------------
GitHub'da "+" > New repository:
  Ad: tamam-ulan-asker
  Private sec (oyun dosyalari telifli olabilir, public yapma)
  README ekleme, bos birak. Create.

ADIM 3 - Projeyi telefondan repoya yukle (Termux)
-------------------------------------------------
1) F-Droid'den Termux kur (Play Store surumu eski, F-Droid kullan).
2) Bu sohbetten TAMAM_ULAN_ASKER_UnityProject.zip dosyasini telefona indir
   (Download klasorune duser).
3) Termux'ta sirayla:
     pkg update -y && pkg install -y git unzip
     termux-setup-storage
     cd ~/storage/downloads
     unzip TAMAM_ULAN_ASKER_UnityProject.zip
     cd TAMAM_ULAN_ASKER_UnityProject
     git init
     git config user.email "EPOSTAN"
     git config user.name "KULLANICIADIN"
     git add -A
     git commit -m "Tamam Ulan Asker mobil proje"
     git branch -M main
     git remote add origin https://github.com/KULLANICIADIN/tamam-ulan-asker.git
     git push -u origin main
   Push sirasinda sifre yerine GitHub Personal Access Token ister:
   GitHub > Settings > Developer settings > Personal access tokens (classic)
   > "repo" kutusunu isaretleyip token olustur, Termux'a yapistir.
   (Tokeni kimseyle paylasma.)

ADIM 4 - Unity lisansi (GameCI icin)
-------------------------------------
1) Repoda Actions sekmesi > "Unity Lisans Aktivasyon Dosyasi Al" >
   Run workflow. Bitince sayfanin altindan artifact'i (.alf dosyasi) indir.
2) license.unity3d.com adresine Unity hesabinla giris yap,
   .alf dosyasini yukle, sana verilen .ulf lisans dosyasini indir.
3) GitHub repo > Settings > Secrets and variables > Actions >
   New repository secret:
     Ad: UNITY_LICENSE
     Deger: .ulf dosyasinin icerigi (dosyayi Termux'ta `cat Unity_v2019.4.15f1.ulf`
     yazip cikan metnin tamamini kopyala-yapistir)
   Istege bagli ayni sekilde UNITY_EMAIL ve UNITY_PASSWORD sirlarini da
   kendi Unity hesap bilgilerinle ekle (sadece GitHub sirrina yaz,
   sohbete degil).

ADIM 5 - APK build
------------------
1) Actions > "Android APK Build" > Run workflow.
2) Ilk build 20-60 dakika surebilir. Bitince ayni sayfadan
   "TamamUlanAsker-APK" artifact'ini indir, icindeki .apk'yi kur.
   (Bilinmeyen kaynaklara izin vermen gerekebilir.)

HATA OLURSA
-----------
Actions kirmizi olursa isin ustine dokun, en alttaki kirmizi hata
satirlarinin ekran goruntusunu bana gonder. Buyuk ihtimalle proje
kaynakli bir derleme hatasidir, burada duzeltip yeni zip yollarim;
sen Termux'ta yeni dosyalari ayni repoya tekrar push edersin.

NOTLAR
------
- GitHub Free'de Private repo icin ayda 2000 dakika Actions hakki var;
  birkac build icin yeter.
- Unity'nin kendi Cloud Build servisi de benzer bir yoldur ama bu
  GameCI yolu ucretsiz oldugu icin bunu hazirladim.

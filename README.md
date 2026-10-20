# TAMAM ULAN ASKER - Mobil Proje (Unity 2019.4.15f1)

Bu repo GitHub Actions ile **PC'siz telefondan APK** almak için hazırlandı.

## Hızlı Başlangıç (Telefondan)

Detaylı rehber: `TELEFONDAN_APK.md` dosyasına bak.

Özet:
1. **Unity Lisansı al:** Actions > "Unity Lisans Aktivasyon Dosyasi Al" > Run workflow > çıkan `.alf` dosyasını indir > https://license.unity3d.com'a yükle > `.ulf` dosyasını al > Repo Settings > Secrets > `UNITY_LICENSE` olarak ekle
2. **APK Build:** Actions > "Android APK Build" > Run workflow > bitince artifact'ten APK indir

## Unity Sürümü
- `2019.4.15f1` (Tam bu sürüm gerekli)

## Build Çıktısı
- `Builds/TamamUlanAsker.apk`


GITHUB ACTIONS WORKFLOW KURULUMU (30 saniye)
============================================
GitHub App workflow dosyasini direkt push'lamama izin vermiyor, bu yuzden
workflow'lari buraya koydum. Senin tek yapman gereken:

1. GitHub'da repona gir: https://github.com/erenbeyefendi470-hue/Askerrre
2. Arena branch'ini main'e merge et (veya direk bu branch'te calis)
3. Asagidaki adimlari telefondan yap:

YONTEM A (En kolay - Telefondan):
- Repo ana sayfasinda "Add file" > "Create new file"
- Dosya adina yaz: .github/workflows/build-android.yml
- Icerigine WORKFLOW_KURULUM/build-android.yml dosyasinin icerigini yapistir
- Ayni sekilde .github/workflows/activation.yml icin de yap
- Commit

YONTEM B (Termux ile):
Termux'ta repo klasorunde:
  mkdir -p .github/workflows
  mv WORKFLOW_KURULUM/*.yml .github/workflows/
  git add .github/workflows
  git commit -m "workflows eklendi"
  git push

Sonra Actions sekmesinden build'i baslatabilirsin.


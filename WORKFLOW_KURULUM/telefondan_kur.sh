#!/bin/bash
# Telefondan workflow kurulum betigi - Termux'ta calistir
# Kullanim: bash WORKFLOW_KURULUM/telefondan_kur.sh

echo "Workflow dosyalari kuruluyor..."
mkdir -p .github/workflows
cp WORKFLOW_KURULUM/*.yml .github/workflows/
echo "Dosyalar tasindi:"
ls -lh .github/workflows/
echo ""
echo "Git'e ekleniyor..."
git add .github/workflows/
git commit -m "Add GitHub Actions workflows for Android APK build"
git push
echo ""
echo "Bitti! Simdi GitHub'da Actions sekmesine bakabilirsin."

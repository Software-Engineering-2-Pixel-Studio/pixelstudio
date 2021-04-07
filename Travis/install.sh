#! /bin/sh

# Download Unity3D installer into the container
#  The below link will need to change depending on the version, this one is for 5.5.1
#  Refer to https://unity3d.com/get-unity/download/archive and find the link pointed to by Mac "Unity Editor"
echo 'Downloading Unity 2020.2.3 pkg:'
curl --retry 5 -o Unity.pkg https://download.unity3d.com/download_unity/8ff31bc5bf5b/MacEditorInstaller/Unity.pkg?_ga=2.163280124.1577011882.1617643140-2007202143.1617643140
if [ $? -ne 0 ]; then { echo "Download failed"; exit $?; } fi

# Run installer(s)
echo 'Installing Unity.pkg'
sudo installer -dumplog -package Unity.pkg -target /

#! /bin/sh

## Run the editor unit tests
echo "Running editor unit tests for ${UNITYCI_PROJECT_NAME}"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
	-batchmode \
	-nographics \
	-silent-crashes \
	-logFile $(pwd)/unity.log \
	-projectPath "$(pwd)/${UNITYCI_PROJECT_NAME}" \
	-runEditorTests \
	-quit

rc0=$?
echo "Unit test logs"

## Make the build for the OSX
echo "Attempting build of ${UNITYCI_PROJECT_NAME} for OSX"
/Applications/Unity/Unity.app/Contents/MacOS/Unity \
	-batchmode \
	-nographics \
	-silent-crashes \
	-logFile $(pwd)/unity.log \
	-projectPath "$(pwd)/${UNITYCI_PROJECT_NAME}" \
	-buildOSXUniversalPlayer "$(pwd)/Build/osx/${UNITYCI_PROJECT_NAME}.app" \
	-quit

rc1=$?
echo "Build logs (OSX)"
cat $(pwd)/unity.log

exit $($rc1)
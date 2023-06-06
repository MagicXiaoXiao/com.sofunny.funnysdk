cp -rf ../../Assets/Samples/FunnySDK/0.47.2/FunnySDKSample/* Samples~/FunnySDKSample
git add Samples~/* -f
commitMsg="$1"
echo "commitMsg$commitMsg"
git commit -m $commitMsg
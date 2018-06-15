# gumgum

## Unityのバージョン
unity version 5.5.4f1

## 実験をする上でのメモ
* 腕が伸びる長さと時間の設定
LocalAvatar→VisibleAvatar→VisibleAvatarScript→StretchDegreeとStretchTotalTimeを編集
** StretchDegree
RightHandとRightElbowの間を何倍に伸ばすかを決める値
** StretchTotalTime
腕が伸びきるのにかかる時間

* 各機構の有無の設定
Motor→MotorController→MechanismModeを編集
** MechanismMode
0:両方あり, 1:重心移動のみ, 2:皮膚伸ばしのみ

## デバッグをする上でのメモ
* 右手のコントローラーの代わりに左手のコントローラーを使用
LocalAvatar→InvisibleAvatar→VRIK(Script)→Right Arm→TargetにLeft Hand Targetを指定
InvisibleAvatarScript.csのUpdateメソッド→handAccel=の行→OVRInput.Controller.LTouchにする(2箇所ある)

## 編集したいところ
* コメントアウトでmustって書いてあるところ

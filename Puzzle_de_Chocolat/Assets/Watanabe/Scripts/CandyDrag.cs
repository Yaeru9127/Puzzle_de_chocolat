using UnityEngine;

public class CandyDrag : MonoBehaviour
{
    // マウスとオブジェクトの距離を保持するためのオフセット
    private Vector3 offset;

    // マウスをクリックしたときのワールド座標
    private Vector3 mouseDownPos;

    // 現在ドラッグ中かどうかのフラグ
    private bool isDragging = false;

    // 最後の操作がドラッグかどうかを外部から参照できるプロパティ
    public bool IsDragged { get; private set; } = false;

    // 残機管理スクリプトの参照（インスペクターからアタッチ）
    public Remainingaircraft remainingAircraft;

    // マウスボタンを押したときに呼ばれる
    void OnMouseDown()
    {
        // マウスの現在位置（ワールド座標）を取得
        mouseDownPos = GetMouseWorldPos();

        // マウス位置とオブジェクト位置の差をオフセットとして保存
        offset = transform.position - mouseDownPos;

        // ドラッグ状態を初期化
        isDragging = false;
        IsDragged = false;
    }

    // マウスをドラッグしている間、毎フレーム呼ばれる
    void OnMouseDrag()
    {
        // 現在のマウス位置を取得
        Vector3 currentMousePos = GetMouseWorldPos();

        // オブジェクトをマウスの位置＋オフセットに移動させる
        transform.position = currentMousePos + offset;

        // ドラッグ判定：一定以上マウスが動いたらドラッグと判定
        if (!isDragging && Vector3.Distance(currentMousePos, mouseDownPos) > 0.1f)
        {
            isDragging = true;
            IsDragged = true;
        }
    }

    // マウスボタンを離したときに呼ばれる
    void OnMouseUp()
    {
        // ドラッグが行われていた場合、残機を減らす
        if (isDragging && remainingAircraft != null)
        {
            remainingAircraft.ReduceLife();
        }

        // ドラッグ状態をリセット
        isDragging = false;
    }

    // マウスのスクリーン座標をワールド座標に変換する
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;

        // Z座標をカメラからの距離に設定
        mousePos.z = -Camera.main.transform.position.z;

        // スクリーン座標をワールド座標に変換して返す
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}

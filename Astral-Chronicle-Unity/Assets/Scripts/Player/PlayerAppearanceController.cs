using UnityEngine;

public class PlayerAppearanceController : MonoBehaviour
{
    // 髪、顔、体などのパーツをSpriteRendererで保持
    public SpriteRenderer hairRenderer;
    public SpriteRenderer faceRenderer;
    public SpriteRenderer bodyRenderer;
    // 他のパーツもここに追加

    // PlayerAppearanceDataに基づいて見た目を更新するメソッド
    public void UpdateAppearance(PlayerAppearanceData hair, PlayerAppearanceData face, PlayerAppearanceData body)
    {
        if (hairRenderer != null && hair != null)
        {
            hairRenderer.sprite = hair.partSprite;
            hairRenderer.color = hair.partColor;
        }
        if (faceRenderer != null && face != null)
        {
            faceRenderer.sprite = face.partSprite;
            faceRenderer.color = face.partColor;
        }
        if (bodyRenderer != null && body != null)
        {
            bodyRenderer.sprite = body.partSprite;
            bodyRenderer.color = body.partColor;
        }
    }
}
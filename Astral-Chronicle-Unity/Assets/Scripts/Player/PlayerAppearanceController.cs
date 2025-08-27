using UnityEngine;

public class PlayerAppearanceController : MonoBehaviour
{
    // ���A��A�̂Ȃǂ̃p�[�c��SpriteRenderer�ŕێ�
    public SpriteRenderer hairRenderer;
    public SpriteRenderer faceRenderer;
    public SpriteRenderer bodyRenderer;
    // ���̃p�[�c�������ɒǉ�

    // PlayerAppearanceData�Ɋ�Â��Č����ڂ��X�V���郁�\�b�h
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
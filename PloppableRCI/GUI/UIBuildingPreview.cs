﻿using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace PloppableRICO
{
    public class UIBuildingPreview : UIPanel
    {
        private BuildingData m_item;
        private BuildingInfo m_renderPrefab;

        private UITextureSprite m_preview;
        private UISprite m_noPreview;
        private UIPreviewRenderer m_previewRenderer;

        private UILabel m_buildingName;
        private UISprite m_categoryIcon;

        private UILabel m_level;
        private UILabel m_size;

        public override void Start()
        {
            base.Start();

            backgroundSprite = "GenericPanel";

            // Preview
            m_preview = AddUIComponent<UITextureSprite>();
            m_preview.size = size;
            m_preview.relativePosition = Vector3.zero;

            m_noPreview = AddUIComponent<UISprite>();
            m_noPreview.spriteName = "Niet";
            m_noPreview.relativePosition = new Vector3((width - m_noPreview.spriteInfo.width) / 2, (height - m_noPreview.spriteInfo.height) / 2);

            m_previewRenderer = gameObject.AddComponent<UIPreviewRenderer>();
            m_previewRenderer.Size = m_preview.size * 2; // Twice the size for anti-aliasing

            eventMouseDown += (c, p) =>
            {
                eventMouseMove += RotateCamera;
            };

            eventMouseUp += (c, p) =>
            {
                eventMouseMove -= RotateCamera;
            };

            eventMouseWheel += (c, p) =>
            {
                m_previewRenderer.Zoom -= Mathf.Sign(p.wheelDelta) * 0.25f;
                RenderPreview();
            };

            // Name
            m_buildingName = AddUIComponent<UILabel>();
            m_buildingName.textScale = 0.9f;
            m_buildingName.useDropShadow = true;
            m_buildingName.dropShadowColor = new Color32(80, 80, 80, 255);
            m_buildingName.dropShadowOffset = new Vector2(2, -2);
            m_buildingName.text = "Name";
            m_buildingName.isVisible = false;
            m_buildingName.relativePosition = new Vector3(5, 10);

            // Category icon
            m_categoryIcon = AddUIComponent<UISprite>();
            m_categoryIcon.size = new Vector2(35, 35);
            m_categoryIcon.isVisible = false;
            m_categoryIcon.relativePosition = new Vector3(width - 37, 2);

            // Level
            m_level = AddUIComponent<UILabel>();
            m_level.textScale = 0.9f;
            m_level.useDropShadow = true;
            m_level.dropShadowColor = new Color32(80, 80, 80, 255);
            m_level.dropShadowOffset = new Vector2(2, -2);
            m_level.text = "Level";
            m_level.isVisible = false;
            m_level.relativePosition = new Vector3(5, height - 20);

            // Size
            m_size = AddUIComponent<UILabel>();
            m_size.textScale = 0.9f;
            m_size.useDropShadow = true;
            m_size.dropShadowColor = new Color32(80, 80, 80, 255);
            m_size.dropShadowOffset = new Vector2(2, -2);
            m_size.text = "Size";
            m_size.isVisible = false;
            m_size.relativePosition = new Vector3(width - 50, height - 20);
        }

        public void Show(BuildingData item)
        {
            if (m_item == item) return;

            m_item = item;
            m_renderPrefab = (m_item == null) ? null : (PrefabCollection<BuildingInfo>.FindLoaded(m_item.name));

            // Preview
            if (m_renderPrefab != null && m_renderPrefab.m_mesh != null)
            {
                m_previewRenderer.CameraRotation = 210f;
                m_previewRenderer.Zoom = 4f;
                m_previewRenderer.Mesh = m_renderPrefab.m_mesh;
                m_previewRenderer.material = m_renderPrefab.m_material;

                RenderPreview();

                m_preview.texture = m_previewRenderer.Texture;

                m_noPreview.isVisible = false;
            }
            else
            {
                m_preview.texture = null;
                m_noPreview.isVisible = true;
            }

            m_buildingName.isVisible = false;
            m_categoryIcon.isVisible = false;
            m_level.isVisible = false;
            m_size.isVisible = false;

            if (item == null) return;

            // Name
            m_buildingName.isVisible = true;
            m_buildingName.text = m_item.displayName;
            UIUtils.TruncateLabel(m_buildingName, width - 45);
            m_buildingName.autoHeight = true;

            // Level.
            m_level.isVisible = true;
            m_level.text = Translations.GetTranslation("Level") + " " + ((int)m_item.prefab.m_class.m_level + 1);
            UIUtils.TruncateLabel(m_level, width - 45);
            m_level.autoHeight = true;

            // Size.
            m_size.isVisible = true;
            m_size.text = m_item.prefab.GetWidth() + "x" + m_item.prefab.GetLength();
            UIUtils.TruncateLabel(m_size, width - 45);
            m_size.autoHeight = true;


        }

        private void RenderPreview()
        {
            if (m_renderPrefab == null) return;

            if (m_renderPrefab.m_useColorVariations)
            {
                Color materialColor = m_renderPrefab.m_material.color;
                m_renderPrefab.m_material.color = m_renderPrefab.m_color0;
                m_previewRenderer.Render();
                m_renderPrefab.m_material.color = materialColor;
            }
            else
            {
                m_previewRenderer.Render();
            }
        }

        private void RotateCamera(UIComponent c, UIMouseEventParameter p)
        {
            m_previewRenderer.CameraRotation -= p.moveDelta.x / m_preview.width * 360f;
            RenderPreview();
        }
    }
}
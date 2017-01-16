using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class BiomeCategory : Category
    {
        private SectorManager _sectorManager;
        private BiomeManager _biomeManager;

        private SectorCategory _sectorCategory;

        private string[] _biomeOptions = null;
        private int _biomeIndex = nothingIndex;
        private const int numberAdditionalBiomeOptions = 2;
        private const int everythingIndex = 0;
        private const int nothingIndex = 1;

        private enum DisplayOption
        {
            Classic,
            Splitted,
            Weighted
        }

        private bool _drawDistance;
        private bool _drawColor;

        private DisplayOption _displayOption;

        public BiomeCategory(EditorWindow window, SectorCategory sectorCategory, bool startingHidden = false)
            : base(window, "Biome", startingHidden)
        {
            _sectorCategory = sectorCategory;
        }

        protected override void DrawContent()
        {
            if (_biomeOptions != null)
            {
                _biomeIndex = EditorGUILayout.Popup(_biomeIndex, _biomeOptions);
                _displayOption = (DisplayOption) EditorGUILayout.EnumPopup(_displayOption);
            }

            _drawDistance = EditorGUILayout.Toggle("Draw Distance", _drawDistance);

            _drawColor = EditorGUILayout.Toggle("Draw Color", _drawColor);
        }

        protected override bool TryToInit()
        {
            _sectorManager = GameObject.FindGameObjectWithTag("SectorManager").GetComponent<SectorManager>();

            _biomeManager = GameObject.FindGameObjectWithTag("BiomeManager").GetComponent<BiomeManager>();

            if (!_sectorManager.IsInitialized() || !_biomeManager.IsInitialized())
            {
                return false;
            }

            Biome[] biomes = _biomeManager.biomes;

            _biomeOptions = new string[biomes.Length + numberAdditionalBiomeOptions];

            _biomeOptions[everythingIndex] = "Everything";
            _biomeOptions[nothingIndex] = "Nothing";

            for (int i = 0; i < biomes.Length; i++)
            {
                _biomeOptions[i + numberAdditionalBiomeOptions] = biomes[i].name;
            }

            return true;
        }

        protected override void UpdateContent()
        {
            // todo
        }

        protected override void ResetContent()
        {
        }

        public override void DrawGizmosContent()
        {
            if (_drawColor && _biomeIndex != nothingIndex)
            {
                DrawBiomeColor();
            }

            if (_drawDistance && _biomeIndex != nothingIndex)
            {
                DrawDistance();
            }
        }

        private void DrawBiomeColor()
        {
            List<Sector> currentSectors =
                _sectorManager.GetAllSectors()[_sectorCategory.GetResolutionIndex()].singleEntryList;

            // check if biomes are setted
            if (currentSectors[0].GetBiomeList() == null)
            {
                return;
            }

            //Rect bufferGUIRect;

            foreach (Sector sector in currentSectors)
            {
                List<BiomeContainer> biomeContainers = sector.GetBiomeList().GetBiomeContainers();

                if (biomeContainers == null || biomeContainers.Count == 0)
                    return;

                for (int i = 0; i < biomeContainers.Count; i++)
                {
                    if (_biomeIndex == everythingIndex
                        || biomeContainers[i].biome == _biomeManager.biomes[_biomeIndex - numberAdditionalBiomeOptions])
                    {
                        switch (_displayOption)
                        {
                            case DisplayOption.Classic:
                                //bufferGUIRect = GizmosHelper.ConvertToGUICoordinate(sector.GetBounds());
                                //GizmosHelper.DrawFillRect(bufferGUIRect, biomeContainers[i].biome.data.color);
                                GizmosHelper.DrawFillRect(sector.GetBounds(), biomeContainers[i].biome.data.color);
                                break;

                            case DisplayOption.Splitted:
                                Rect newSplittedBounds = ComputeRectFromBiomeCount(biomeContainers.Count, i,
                                    sector.GetBounds());
                                //bufferGUIRect = GizmosHelper.ConvertToGUICoordinate(newSplittedBounds);
                                //GizmosHelper.DrawFillRect(bufferGUIRect, biomeContainers[i].biome.data.color);
                                GizmosHelper.DrawFillRect(newSplittedBounds, biomeContainers[i].biome.data.color);
                                break;

                            case DisplayOption.Weighted:
                                Rect newWeightedBounds = ComputeRectFromBiomeRatio(biomeContainers[i],
                                    sector.GetBounds());
                                //bufferGUIRect = GizmosHelper.ConvertToGUICoordinate(newWeightedBounds);
                                //GizmosHelper.DrawFillRect(bufferGUIRect, biomeContainers[i].biome.data.color);
                                GizmosHelper.DrawFillRect(newWeightedBounds, biomeContainers[i].biome.data.color);
                                break;
                        }
                    }
                }
            }
        }

        private Rect ComputeRectFromBiomeRatio(BiomeContainer biomeContainer, Rect sectorRect)
        {
            float width = sectorRect.width*biomeContainer.ratio;
            float offset = sectorRect.width*biomeContainer.offsetRatio;

            Rect newRect = new Rect(sectorRect);

            newRect.width = width;
            newRect.x += offset;

            return newRect;
        }

        private Rect ComputeRectFromBiomeCount(int biomeCount, int index, Rect sectorRect)
        {
            float width = sectorRect.width/biomeCount;
            float offset = width*index;

            Rect newRect = new Rect(sectorRect);

            newRect.width = width;
            newRect.x += offset;

            return newRect;
        }

        private void DrawDistance()
        {
            Biome[] biomes = _biomeManager.biomes;

            //Vector2 GUIPosition;

            for (int i = 0; i < biomes.Length; i++)
            {
                if (_biomeIndex == everythingIndex || _biomeIndex == i)
                {
                    Dictionary<int, List<Sector>> sortedSectors =
                        biomes[i].GetSortedSectors(_sectorCategory.GetResolutionIndex());

                    if (sortedSectors != null)
                    {
                        foreach (int level in sortedSectors.Keys)
                        {
                            for (int sectorIndex = 0; sectorIndex < sortedSectors[level].Count; sectorIndex++)
                            {
                                //GUIPosition = GizmosHelper.ConvertToGUICoordinate(sortedSectors[level][sectorIndex].GetPosition());

                                //Handles.Label(GUIPosition, level.ToString());
                                GizmosHelper.DrawLabel(sortedSectors[level][sectorIndex].GetPosition(), 
                                    level.ToString());
                            }
                        }
                    }
                }
            }
        }
    }
}
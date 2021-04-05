﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace MonoGame_LDtk_Importer
{
    public class Reader1 : ContentTypeReader<LDtkProject>
    {
        protected override LDtkProject Read(ContentReader input, LDtkProject existingInstance)
        {
            LDtkProject project = new LDtkProject();

            project.BackgroundColor = input.ReadColor();
            project.WorldGridHeight = input.ReadInt32();
            project.WorldGridWidth = input.ReadInt32();
            project.WorldLayout = (worldLayoutTypes)input.ReadInt32();

            #region definitions

            project.Definitions = new Definitions();
            //entities
            project.Definitions.Entities = new List<EntitieDef>();
            int entitieDefsCount = input.ReadInt32();
            for (int i = 0; i < entitieDefsCount; i++)
            {
                EntitieDef entitie = new EntitieDef();
                entitie.Color = input.ReadColor();
                entitie.Height = input.ReadInt32();
                entitie.Width = input.ReadInt32();
                entitie.Identifier = input.ReadString();
                entitie.PivotCoordinates = input.ReadVector2();
                if (input.ReadBoolean()) entitie.TileId = input.ReadInt32();
                if (input.ReadBoolean()) entitie.TilesetId = input.ReadInt32();
                entitie.Uid = input.ReadInt32();

                project.Definitions.Entities.Add(entitie);
            }

            //enums
            project.Definitions.Enums = new List<EnumDef>();
            int enumDefsCount = input.ReadInt32();
            for (int i = 0; i < enumDefsCount; i++)
            {
                EnumDef enumDef = new EnumDef();
                if (input.ReadBoolean()) enumDef.ExternalRelPath = input.ReadString();
                if (input.ReadBoolean()) enumDef.IconTilesetUid = input.ReadInt32();
                enumDef.Identifier = input.ReadString();
                enumDef.Uid = input.ReadInt32();

                enumDef.Values = new List<EnumValueDef>();
                int enumDefValueNb = input.ReadInt32();
                for (int j = 0; j < enumDefValueNb; j++)
                {
                    EnumValueDef value = new EnumValueDef();
                    value.TileSourceRectangle = new Rectangle(
                        input.ReadInt32(),
                        input.ReadInt32(),
                        input.ReadInt32(),
                        input.ReadInt32()
                        );
                    value.Id = input.ReadString();
                    if (input.ReadBoolean()) value.TileId = input.ReadInt32();
                    enumDef.Values.Add(value);
                }
                project.Definitions.Enums.Add(enumDef);
            }

            //externals enums
            project.Definitions.ExternalEnums = new List<EnumDef>();
            int externalEnumDefsCount = input.ReadInt32();
            for (int i = 0; i < externalEnumDefsCount; i++)
            {
                EnumDef enumDef = new EnumDef();
                if (input.ReadBoolean()) enumDef.ExternalRelPath = input.ReadString();
                if (input.ReadBoolean()) enumDef.IconTilesetUid = input.ReadInt32();
                enumDef.Identifier = input.ReadString();
                enumDef.Uid = input.ReadInt32();

                enumDef.Values = new List<EnumValueDef>();
                int enumDefValueNb = input.ReadInt32();
                for (int j = 0; j < enumDefValueNb; j++)
                {
                    EnumValueDef value = new EnumValueDef();
                    value.TileSourceRectangle = new Rectangle(
                        input.ReadInt32(),
                        input.ReadInt32(),
                        input.ReadInt32(),
                        input.ReadInt32()
                        );
                    value.Id = input.ReadString();
                    if (input.ReadBoolean()) value.TileId = input.ReadInt32();
                    enumDef.Values.Add(value);
                }
                project.Definitions.Enums.Add(enumDef);
            }

            //layers
            project.Definitions.Layers = new List<LayerDef>();
            int layerDefsCount = input.ReadInt32();
            for (int i = 0; i < layerDefsCount; i++)
            {
                LayerDef layer = new LayerDef();

                layer.Type = (LayerType)input.ReadInt32();
                if (input.ReadBoolean()) layer.AutoSourceLayerDefUid = input.ReadInt32();
                if (input.ReadBoolean()) layer.AutoTilesetDefUid = input.ReadInt32();
                layer.DisplayOpacity = input.ReadSingle();
                layer.GridSize = input.ReadInt32();
                layer.Identifier = input.ReadString();
                layer.Offset = input.ReadVector2();
                if (input.ReadBoolean()) layer.TilesetDefUid = input.ReadInt32();
                layer.Uid = input.ReadInt32();

                layer.IntGridValues = new List<IntGridValueDef>();
                int intGridValuesCount = input.ReadInt32();
                for (int j = 0; j < intGridValuesCount; j++)
                {
                    IntGridValueDef intGridValue = new IntGridValueDef();
                    intGridValue.Color = input.ReadColor();
                    if (input.ReadBoolean()) intGridValue.Identifier = input.ReadString();
                    intGridValue.Value = input.ReadInt32();
                    layer.IntGridValues.Add(intGridValue);
                }
                project.Definitions.Layers.Add(layer);
            }

            //tilesets
            project.Definitions.Tilesets = new List<Tileset>();
            int tilesetCount = input.ReadInt32();
            for (int i = 0; i < tilesetCount; i++)
            {
                Tileset tileset = new Tileset();
                tileset.Identifier = input.ReadString();
                tileset.Padding = input.ReadInt32();
                tileset.Height = input.ReadInt32();
                tileset.Width = input.ReadInt32();
                tileset.RelPath = input.ReadString();
                tileset.Spacing = input.ReadInt32();
                tileset.TileGridSize = input.ReadInt32();
                tileset.Uid = input.ReadInt32();
            }

            #endregion

            #region levels

            int levelCount = input.ReadInt32();
            for (int i = 0; i < 0; i++)
            {
                Level level = new Level();
                level.BackgroundColor = input.ReadColor();
                if (input.ReadBoolean()) level.BackgroundRelPath = input.ReadString();
                level.Identifier = input.ReadString();
                level.Height = input.ReadInt32();
                level.Width = input.ReadInt32();
                level.Uid = input.ReadInt32();
                level.WorldCoordinates = input.ReadVector2();

                //background position fields
                if (input.ReadBoolean())
                {
                    level.BackgroundPosition = new BackgroundPosition(
                        new Rectangle(
                            input.ReadInt32(),
                            input.ReadInt32(),
                            input.ReadInt32(),
                            input.ReadInt32()
                            ),
                        input.ReadVector2(),
                        input.ReadVector2()
                        );
                }

                //neighbours
                level.Neighbours = new List<LevelNeighbour>();
                int neighboursCount = input.ReadInt32();
                for (int j = 0; j < neighboursCount; j++)
                {
                    LevelNeighbour neighbour = new LevelNeighbour();
                    neighbour.Direction = (NeighbourDirection)input.ReadInt32();
                    neighbour.LevelUid = input.ReadInt32();
                    level.Neighbours.Add(neighbour);
                }

                //field instances
                level.FieldInstances = new List<FieldInstance>();
                int fieldInstancesCount = input.ReadInt32();
                for (int j = 0; j < fieldInstancesCount; j++)
                {
                    FieldInstance field = new FieldInstance();
                    field.Identifier = input.ReadString();
                    field.Type = (FieldType)input.ReadInt32();
                    field.Value = input.ReadString();
                    field.DefUid = input.ReadInt32();
                    if (input.ReadBoolean()) field.EnumName = input.ReadString();
                    field.IsArray = input.ReadBoolean();

                    level.FieldInstances.Add(field);
                }

                //layers
                level.LayerInstances = new List<LayerInstance>();
                int layersCount = input.ReadInt32();
                for (int j = 0; j < layersCount; j++)
                {
                    LayerInstance layer = new LayerInstance();
                    layer.Height = input.ReadInt32();
                    layer.Width = input.ReadInt32();
                    layer.GridSize = input.ReadInt32();
                    layer.Identifier = input.ReadString();
                    layer.Opacity = input.ReadSingle();
                    layer.TotalOffset = input.ReadVector2();
                    if (input.ReadBoolean()) layer.TilesetDefUid = input.ReadInt32();
                    if (input.ReadBoolean()) layer.TilesetRelPath = input.ReadString();
                    layer.Type = (LayerType)input.ReadInt32();
                    layer.LayerDefUid = input.ReadInt32();
                    layer.LevelId = input.ReadInt32();
                    if (input.ReadBoolean()) layer.OverrideTilesetUid = input.ReadInt32();
                    layer.Offset = input.ReadVector2();
                    layer.IsVisible = input.ReadBoolean();

                    //auto layer tiles
                    layer.AutoLayerTiles = new List<TileInstance>();
                    int autoTilesCount = input.ReadInt32();
                    for (int k = 0; k < autoTilesCount; k++)
                    {
                        TileInstance tile = new TileInstance();
                        tile.IsFlippedOnX = input.ReadBoolean();
                        tile.IsFlippedOnY = input.ReadBoolean();
                        tile.Coordinates = input.ReadVector2();
                        tile.Source = input.ReadVector2();
                        tile.TileId = input.ReadInt32();

                        layer.AutoLayerTiles.Add(tile);
                    }

                    //entities
                    layer.EntityInstances = new List<EntityInstance>();
                    int entitieCount = input.ReadInt32();
                    for (int k = 0; k < entitieCount; k++)
                    {
                        EntityInstance entity = new EntityInstance();
                        entity.GridCoordinates = input.ReadVector2();
                        entity.Identifier = input.ReadString();
                        entity.PivotCoordinates = input.ReadVector2();
                        entity.DefUid = input.ReadInt32();
                        entity.Height = input.ReadInt32();
                        entity.Width = input.ReadInt32();
                        entity.Coordinates = input.ReadVector2();

                        if (input.ReadBoolean())
                        {
                            EntityTile tile = new EntityTile();
                            tile.SourceRectangle = new Rectangle(
                                input.ReadInt32(),
                                input.ReadInt32(),
                                input.ReadInt32(),
                                input.ReadInt32()
                                );
                            tile.TilesetUid = input.ReadInt32();
                            entity.Tile = tile;
                        }

                        //field instances
                        entity.FieldInstances = new List<FieldInstance>();
                        int fieldInstancesCount2 = input.ReadInt32();
                        for (int l = 0; j < fieldInstancesCount2; l++)
                        {
                            FieldInstance field = new FieldInstance();
                            field.Identifier = input.ReadString();
                            field.Type = (FieldType)input.ReadInt32();
                            field.Value = input.ReadString();
                            field.DefUid = input.ReadInt32();
                            field.EnumName = input.ReadString();
                            field.IsArray = input.ReadBoolean();

                            entity.FieldInstances.Add(field);
                        }
                    }

                    //grid tiles
                    layer.GridTilesInstances = new List<TileInstance>();
                    int gridTilesCount = input.ReadInt32();
                    for (int k = 0; k < gridTilesCount; k++)
                    {
                        TileInstance tile = new TileInstance();
                        tile.IsFlippedOnX = input.ReadBoolean();
                        tile.IsFlippedOnY = input.ReadBoolean();
                        tile.Coordinates = input.ReadVector2();
                        tile.Source = input.ReadVector2();
                        tile.TileId = input.ReadInt32();

                        layer.AutoLayerTiles.Add(tile);
                    }

                    layer.IntGridCsv = new int[input.ReadInt32()];
                    for (int k = 0; k < layer.IntGridCsv.Length; k++)
                    {
                        layer.IntGridCsv.SetValue(input.ReadInt32(), k);
                    }
                }
            }

            #endregion

            return project;
        }
    }
}
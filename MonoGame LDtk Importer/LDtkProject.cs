﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MonoGame_LDtk_Importer
{
    /// <summary>
    /// A LDtk project containing layers definitions and levels
    /// </summary>
    public class LDtkProject
    {
        /// <summary>
        /// Project background color
        /// </summary>
        public Color BackgroundColor { get; set; }
        /// <summary>
        /// A structure containing all the definitions of this project
        /// </summary>
        public Definitions Definitions { get; set; }

        //External levels are loaded automatically, no need to load the bool value

        /// <summary>
        /// All levels. The order of this array is only relevant in LinearHorizontal and
        /// linearVertical world layouts (see worldLayout value).
        /// Otherwise, you should refer to the worldX, worldY coordinates of each Level.
        /// </summary>
        public List<Level> Levels { get; set; }
        /// <summary>
        /// Height of the world grid in pixels.
        /// </summary>
        public int WorldGridHeight { get; set; }
        /// <summary>
        /// Width of the world grid in pixels.
        /// </summary>
        public int WorldGridWidth { get; set; }
        /// <summary>
        /// An enum that describes how levels are organized in this project (ie. linearly or in a 2D space).
        /// Possible values are: Free, GridVania, LinearHorizontal and LinearVertical.
        /// </summary>
        public worldLayoutTypes WorldLayout { get; set; }

        /// <summary>
        /// Load the main project
        /// </summary>
        /// <param name="project">A json element containing the project</param>
        /// <returns></returns>
        public static LDtkProject LoadProject(JsonElement project)
        {
            LDtkProject output = new LDtkProject();

            foreach (JsonProperty property in project.EnumerateObject().ToArray())
            {
                if (property.Value.ValueKind != JsonValueKind.Null)
                {
                    if (property.Name == "bgColor")
                    {
                        output.BackgroundColor = new Color(
                                System.Drawing.ColorTranslator.FromHtml(property.Value.GetString()).R,
                                System.Drawing.ColorTranslator.FromHtml(property.Value.GetString()).G,
                                System.Drawing.ColorTranslator.FromHtml(property.Value.GetString()).B
                                );
                    }
                    else if (property.Name == "defs")
                    {
                        output.Definitions = Definitions.LoadDefinitions(property);
                    }
                    else if (property.Name == "levels")
                    {
                        output.Levels = Level.LoadLevels(property);
                    }
                    else if (property.Name == "worldGridHeight")
                    {
                        output.WorldGridHeight = property.Value.GetInt32();
                    }
                    else if (property.Name == "worldGridWidth")
                    {
                        output.WorldGridWidth = property.Value.GetInt32();
                    }
                    else if (property.Name == "worldLayout")
                    {
                        output.WorldLayout = (worldLayoutTypes)Enum.Parse(typeof(worldLayoutTypes), property.Value.GetString());
                    }
                }
            }
            return output;
        }
    }

    /// <summary>
    /// Types of the world layout
    /// </summary>
    public enum worldLayoutTypes
    {
        Free,
        GridVania,
        LinearHorizontal,
        LinearVertical
    }



    /// <summary>
    /// A level
    /// </summary>
    public struct Level
    {
        /// <summary>
        /// Background color of the level. If null, the project defaultLevelBgColor should be used.
        /// </summary>
        public Color BackgroundColor { get; set; }
        /// <summary>
        /// Position informations of the background image, if there is one
        /// </summary>
        public BackgroundPosition? BackgroundPosition { get; set; }
        /// <summary>
        /// An array listing all other levels touching this one on the world map
        /// </summary>
        public List<LevelNeighbour> Neighbours { get; set; }
        /// <summary>
        /// The <i>optional</i> relative path to the level background image
        /// </summary>
        public string BackgroundRelPath { get; set; }

        //The externalRelPath value is used to load the level only, no need to load it.

        /// <summary>
        /// An array containing all Layer instances.
        /// <b>IMPORTANT</b>: if the project option "<i>Save levels separately</i>" is enabled, this field will be null.<br/>
        /// This array is <b>sorted in display order</b>: the 1st layer is the top-most and the last is behind
        /// </summary>
        public List<FieldInstance> FieldInstances { get; set; }
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// All the layers of the level
        /// </summary>
        public List<LayerInstance> LayerInstances { get; set; }
        /// <summary>
        /// Height of the level in pixels
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Width of the level in pixels
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        public int Uid { get; set; }
        /// <summary>
        /// World coordinates in pixels
        /// </summary>
        public Vector2 WorldCoordinates { get; set; }

        /// <summary>
        ///  Load the levels of project
        /// </summary>
        /// <param name="jsonProperty">A json property containing the entities instances</param>
        /// <returns></returns>
        public static List<Level> LoadLevels(JsonProperty jsonProperty)
        {
            List<Level> output = new List<Level>();
            foreach (JsonElement jsonElement in jsonProperty.Value.EnumerateArray().ToArray())
            {
                Level level = new Level();
                foreach (JsonProperty property in jsonElement.EnumerateObject().ToArray())
                {
                    if (property.Value.ValueKind != JsonValueKind.Null)
                    {
                        if (property.Name == "_bgcolor")
                        {
                            level.BackgroundColor = new Color(
                                System.Drawing.ColorTranslator.FromHtml(property.Value.GetString()).R,
                                System.Drawing.ColorTranslator.FromHtml(property.Value.GetString()).G,
                                System.Drawing.ColorTranslator.FromHtml(property.Value.GetString()).B
                                );
                        }
                        if (property.Name == "_bgPos")
                        {
                            level.BackgroundPosition = MonoGame_LDtk_Importer.BackgroundPosition.LoadBackgroundPos(property);
                        }
                        else if (property.Name == "__neighbours")
                        {
                            level.Neighbours = LevelNeighbour.LoadLevelNeighbours(property);
                        }
                        else if (property.Name == "bgRelPath")
                        {
                            level.BackgroundRelPath = property.Value.GetString();
                        }
                        // load here externalRelPath
                        else if (property.Name == "bgRelPath")
                        {
                            level.BackgroundRelPath = property.Value.GetString();
                        }
                        else if (property.Name == "fieldInstances")
                        {
                            level.FieldInstances = FieldInstance.LoadFields(property);
                        }
                        else if (property.Name == "identifier")
                        {
                            level.Identifier = property.Value.GetString();
                        }
                        else if (property.Name == "layerInstances")
                        {
                            level.LayerInstances = LayerInstance.LoadLayers(property);
                        }
                        else if (property.Name == "pxHei")
                        {
                            level.Height = property.Value.GetInt32();
                        }
                        else if (property.Name == "pxWid")
                        {
                            level.Width = property.Value.GetInt32();
                        }
                        else if (property.Name == "uid")
                        {
                            level.Uid = property.Value.GetInt32();
                        }
                        else if (property.Name == "worldX")
                        {
                            level.WorldCoordinates = new Vector2(property.Value.GetInt32(), jsonElement.GetProperty("worldY").GetInt32());
                        }
                    }
                }
                output.Add(level);
            }
            return output;
        }
    }

    /// <summary>
    /// Position informations of a background image
    /// </summary>
    public struct BackgroundPosition
    {
        /// <summary>
        /// The coordinates of the cropped sub-rectangle of the displayed background image
        /// </summary>
        public Rectangle CropRectangle { get; set; }
        /// <summary>
        /// A Vector2 containing the scales values of the cropped background image
        /// </summary>
        public Vector2 Scale { get; set; }
        /// <summary>
        /// A Vector2 containing the pixel coordinates of the top-left corner of the cropped background image
        /// </summary>
        public Vector2 Coordinates { get; set; }

        /// <summary>
        ///  Load the levels of project
        /// </summary>
        /// <param name="jsonProperty">A json property containing the entities instances</param>
        /// <returns></returns>
        public static BackgroundPosition LoadBackgroundPos(JsonProperty jsonProperty)
        {
            BackgroundPosition bgPos = new BackgroundPosition();
            foreach (JsonProperty property in jsonProperty.Value.EnumerateObject().ToArray())
            {
                if (property.Value.ValueKind != JsonValueKind.Null)
                {
                    if (property.Name == "cropRect")
                    {
                        bgPos.CropRectangle = new Rectangle(
                            property.Value.EnumerateArray().ToArray()[0].GetInt32(),
                            property.Value.EnumerateArray().ToArray()[1].GetInt32(),
                            property.Value.EnumerateArray().ToArray()[2].GetInt32(),
                            property.Value.EnumerateArray().ToArray()[3].GetInt32());
                    }
                    else if (property.Name == "scale")
                    {
                        bgPos.Scale = new Vector2(
                            property.Value.EnumerateArray().ToArray()[0].GetInt32(),
                            property.Value.EnumerateArray().ToArray()[1].GetInt32());
                    }
                    else if (property.Name == "topLeftPx")
                    {
                        bgPos.Coordinates = new Vector2(
                            property.Value.EnumerateArray().ToArray()[0].GetInt32(),
                            property.Value.EnumerateArray().ToArray()[1].GetInt32());
                    }
                }
            }
            return bgPos;
        }

        public BackgroundPosition(Rectangle cropRectangle, Vector2 scale, Vector2 coordinates) : this()
        {

        }
    }

    /// <summary>
    /// A level neighbour
    /// </summary>
    public struct LevelNeighbour
    {
        /// <summary>
        /// An enum indicating the level location (North, South, West, East)
        /// </summary>
        public NeighbourDirection Direction { get; set; }
        /// <summary>
        /// The Int Identifier of the neighbour level
        /// </summary>
        public int LevelUid { get; set; }

        /// <summary>
        /// Load the levels neighbours of a level
        /// </summary>
        /// <param name="jsonProperty">A json property containing the neighbours</param>
        /// <returns></returns>
        public static List<LevelNeighbour> LoadLevelNeighbours(JsonProperty jsonProperty)
        {
            List<LevelNeighbour> output = new List<LevelNeighbour>();
            foreach (JsonElement jsonElement in jsonProperty.Value.EnumerateArray().ToArray())
            {
                LevelNeighbour neighbour = new LevelNeighbour();
                foreach (JsonProperty property in jsonElement.EnumerateObject().ToArray())
                {
                    if (property.Value.ValueKind != JsonValueKind.Null)
                    {
                        if (property.Name == "dir")
                        {
                            if (property.Value.GetString() == "n")
                            {
                                neighbour.Direction = NeighbourDirection.North;
                            }
                            else if (property.Value.GetString() == "s")
                            {
                                neighbour.Direction = NeighbourDirection.South;
                            }
                            else if (property.Value.GetString() == "e")
                            {
                                neighbour.Direction = NeighbourDirection.East;
                            }
                            else if (property.Value.GetString() == "w")
                            {
                                neighbour.Direction = NeighbourDirection.West;
                            }
                        }
                        else if (property.Name == "levelUid")
                        {
                            neighbour.LevelUid = property.Value.GetInt32();
                        }
                    }
                }
                output.Add(neighbour);
            }
            return output;
        }
    }

    public enum NeighbourDirection
    {
        North,
        South,
        West,
        East
    }

    /// <summary>
    /// A layer instance
    /// </summary>
    public struct LayerInstance
    {
        /// <summary>
        /// Grid-based height
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Grid-based width
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Grid size
        /// </summary>
        public int GridSize { get; set; }
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Layer opacity as Float [0-1]
        /// </summary>
        public float Opacity { get; set; }
        /// <summary>
        /// Total layer X pixel offset, including both instance and definition offsets.
        /// </summary>
        public Vector2 TotalOffset { get; set; }
        /// <summary>
        /// The definition UID of corresponding Tileset, if any.
        /// </summary>
        public int? TilesetDefUid { get; set; }
        /// <summary>
        /// The relative path to corresponding Tileset, if any.
        /// </summary>
        public string TilesetRelPath { get; set; }
        /// <summary>
        /// Layer type (possible values: IntGrid, Entities, Tiles or AutoLayer)
        /// </summary>
        public LayerType Type { get; set; }
        /// <summary>
        /// An array containing all tiles generated by Auto-layer rules.
        /// <br/>The array is already sorted in display order (ie. 1st tile is beneath 2nd, which is beneath 3rd etc.).
        /// <br/><br/>
        /// <b>Note:</b> <i>if multiple tiles are stacked in the same cell as the result of different rules, all tiles behind opaque ones will be discarded.</i>
        /// </summary>
        public List<TileInstance> AutoLayerTiles { get; set; }
        /// <summary>
        /// All the values of a Entity layer
        /// </summary>
        public List<EntityInstance> EntityInstances { get; set; }
        /// <summary>
        /// All the tiles of a Tile layer
        /// </summary>
        public List<TileInstance> GridTilesInstances { get; set; }
        /// <summary>
        /// A list of all values in the IntGrid layer, stored from left to right,
        /// and top to bottom: <b>-1</b> means "empty cell" and IntGrid values start at 0.
        /// <br/>This array size is <b>__cWid</b> x <b>__cHei</b> cells.
        /// </summary>
        public int[] IntGridCsv { get; set; }
        /// <summary>
        /// Reference the Layer definition UID
        /// </summary>
        public int LayerDefUid { get; set; }
        /// <summary>
        /// Reference to the UID of the level containing this layer instance
        /// </summary>
        public int LevelId { get; set; }
        /// <summary>
        /// This layer can use another tileset by overriding the tileset UID here
        /// </summary>
        public int? OverrideTilesetUid { get; set; }
        /// <summary>
        /// Offset in pixels to render this layer, usually 0
        /// <br/><i>(<b>IMPORTANT:</b> this should be added to the LayerDef optional offset)</i>
        /// </summary>
        public Vector2 Offset { get; set; }
        /// <summary>
        /// Layer instance visibility
        /// </summary>
        public bool IsVisible { get; set; }

        public static List<LayerInstance> LoadLayers(JsonProperty jsonProperty)
        {
            List<LayerInstance> output = new List<LayerInstance>();
            foreach (JsonElement jsonElement in jsonProperty.Value.EnumerateArray().ToArray())
            {
                LayerInstance layer = new LayerInstance();
                foreach (JsonProperty property in jsonElement.EnumerateObject().ToArray())
                {
                    if (property.Value.ValueKind != JsonValueKind.Null)
                    {
                        if (property.Name == "__cHei")
                        {
                            layer.Height = property.Value.GetInt32();
                        }
                        else if (property.Name == "__cWid")
                        {
                            layer.Width = property.Value.GetInt32();
                        }
                        else if (property.Name == "__gridSize")
                        {
                            layer.GridSize = property.Value.GetInt32();
                        }
                        else if (property.Name == "__identifier")
                        {
                            layer.Identifier = property.Value.GetString();
                        }
                        else if (property.Name == "__opacity")
                        {
                            layer.Opacity = (float)property.Value.GetDouble();
                        }
                        else if (property.Name == "__pxTotalOffsetX")
                        {
                            layer.TotalOffset = new Vector2(property.Value.GetInt32(), jsonElement.GetProperty("__pxTotalOffsetY").GetInt32());
                        }
                        else if (property.Name == "__tilesetDefUid")
                        {
                            layer.TilesetDefUid = property.Value.GetInt32();
                        }
                        else if (property.Name == "__tilesetRelPath")
                        {
                            layer.TilesetRelPath = property.Value.GetString();
                        }
                        else if (property.Name == "__type")
                        {
                            layer.Type = (LayerType)Enum.Parse(typeof(LayerType), property.Value.GetString());
                        }
                        else if (property.Name == "autoLayerTiles")
                        {
                            layer.AutoLayerTiles = TileInstance.LoadTiles(property);
                        }
                        else if (property.Name == "entityInstances")
                        {
                            layer.EntityInstances = EntityInstance.LoadEntities(property);
                        }
                        else if (property.Name == "gridTiles")
                        {
                            layer.GridTilesInstances = TileInstance.LoadTiles(property);
                        }
                        else if (property.Name == "intGridCsv")
                        {
                            int[] intgrid = new int[property.Value.GetArrayLength()];
                            foreach (JsonElement element in property.Value.EnumerateArray().ToArray())
                            {
                                intgrid.Append(element.GetInt32());
                            }
                            layer.IntGridCsv = intgrid;
                        }
                        else if (property.Name == "levelId")
                        {
                            layer.LevelId = property.Value.GetInt32();
                        }
                        else if (property.Name == "layerDefUid")
                        {
                            layer.LayerDefUid = property.Value.GetInt32();
                        }
                        else if (property.Name == "overrideTilesetUid")
                        {
                            layer.OverrideTilesetUid = property.Value.GetInt32();
                        }
                        else if (property.Name == "pxOffsetX")
                        {
                            layer.Offset = new Vector2(property.Value.GetInt32(), jsonElement.GetProperty("pxOffsetY").GetInt32());
                        }
                        else if (property.Name == "visible")
                        {
                            layer.IsVisible = property.Value.GetBoolean();
                        }
                    }
                }
                output.Add(layer);
            }
            return output;
        }
    }

    /// <summary>
    /// A tile instance
    /// </summary>
    public struct TileInstance
    {
        /// <summary>
        /// True if the tile is flipped on x axis
        /// </summary>
        public bool IsFlippedOnX { get; set; }
        /// <summary>
        /// True if the tile is flipped on y axis
        /// </summary>
        public bool IsFlippedOnY { get; set; }
        /// <summary>
        /// Pixel coordinates of the tile in the layer. Don't forget optional layer offsets, if they exist!
        /// </summary>
        public Vector2 Coordinates { get; set; }
        /// <summary>
        /// Pixel coordinates of the tile in the <b>tileset</b>
        /// </summary>
        public Vector2 Source { get; set; }
        /// <summary>
        /// The <i>Tile ID</i> in the corresponding tileset.
        /// </summary>
        public int TileId { get; set; }

        public static List<TileInstance> LoadTiles(JsonProperty jsonProperty)
        {
            List<TileInstance> output = new List<TileInstance>();
            foreach (JsonElement jsonElement in jsonProperty.Value.EnumerateArray().ToArray())
            {
                TileInstance tile = new TileInstance();
                foreach (JsonProperty property in jsonElement.EnumerateObject().ToArray())
                {
                    if (property.Value.ValueKind != JsonValueKind.Null)
                    {
                        if (property.Name == "f")
                        {
                            int f = property.Value.GetInt32();
                            if (f == 0)
                            {
                                tile.IsFlippedOnX = false;
                                tile.IsFlippedOnY = false;
                            }
                            else if (f == 1)
                            {
                                tile.IsFlippedOnX = true;
                                tile.IsFlippedOnY = false;
                            }
                            else if (f == 2)
                            {
                                tile.IsFlippedOnX = false;
                                tile.IsFlippedOnY = true;
                            }
                            else if (f == 3)
                            {
                                tile.IsFlippedOnX = true;
                                tile.IsFlippedOnY = true;
                            }
                        }
                        else if (property.Name == "px")
                        {
                            tile.Coordinates = new Vector2(property.Value.EnumerateArray().ToArray()[0].GetInt32(), property.Value.EnumerateArray().ToArray()[1].GetInt32());
                        }
                        else if (property.Name == "src")
                        {
                            tile.Source = new Vector2(property.Value.EnumerateArray().ToArray()[0].GetInt32(), property.Value.EnumerateArray().ToArray()[1].GetInt32());
                        }
                        else if (property.Name == "t")
                        {
                            tile.TileId = property.Value.GetInt32();
                        }
                    }
                }
                output.Add(tile);
            }
            return output;
        }
    }

    /// <summary>
    /// An entity instance
    /// </summary>
    public struct EntityInstance
    {
        /// <summary>
        /// Grid-based coordinates
        /// </summary>
        public Vector2 GridCoordinates { get; set; }
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Pivot coordinates (values are from 0 to 1) of the Entity
        /// </summary>
        public Vector2 PivotCoordinates { get; set; }
        /// <summary>
        /// Optional Tile used to display this entity (it could either be the default Entity tile, or some tile provided by a field value, like an Enum)
        /// </summary>
        public EntityTile? Tile { get; set; }
        /// <summary>
        /// Reference of the <b>Entity definition</b> UID
        /// </summary>
        public int DefUid { get; set; }
        /// <summary>
        /// All the fields of the entity
        /// </summary>
        public List<FieldInstance> FieldInstances { get; set; }
        /// <summary>
        /// Entity height in pixels. For non-resizable entities, it will be the same as Entity definition
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Entity width in pixels. For non-resizable entities, it will be the same as Entity definition
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Pixel coordinates. Don't forget optional layer offsets, if they exist!
        /// </summary>
        public Vector2 Coordinates { get; set; }

        public static List<EntityInstance> LoadEntities(JsonProperty jsonProperty)
        {
            List<EntityInstance> output = new List<EntityInstance>();
            foreach (JsonElement jsonElement in jsonProperty.Value.EnumerateArray().ToArray())
            {
                EntityInstance entity = new EntityInstance();
                foreach (JsonProperty property in jsonElement.EnumerateObject().ToArray())
                {
                    if (property.Value.ValueKind != JsonValueKind.Null)
                    {
                        if (property.Name == "__grid")
                        {
                            entity.GridCoordinates = new Vector2(property.Value.EnumerateArray().ToArray()[0].GetInt32(), property.Value.EnumerateArray().ToArray()[1].GetInt32());
                        }
                        else if (property.Name == "__identifier")
                        {
                            entity.Identifier = property.Value.GetString();
                        }
                        else if (property.Name == "__pivot")
                        {
                            entity.PivotCoordinates = new Vector2(property.Value.EnumerateArray().ToArray()[0].GetSingle(), property.Value.EnumerateArray().ToArray()[1].GetSingle());
                        }
                        else if (property.Name == "__tile")
                        {
                            EntityTile entityTile = new EntityTile();
                            entityTile.SourceRectangle = new Rectangle(property.Value.EnumerateArray().ToArray()[0].GetInt32(), property.Value.EnumerateArray().ToArray()[1].GetInt32(), property.Value.EnumerateArray().ToArray()[2].GetInt32(), property.Value.EnumerateArray().ToArray()[3].GetInt32());
                            entityTile.TilesetUid = property.Value.GetInt32();
                            entity.Tile = entityTile;
                        }
                        else if (property.Name == "defUid")
                        {
                            entity.DefUid = property.Value.GetInt32();
                        }
                        else if (property.Name == "fieldInstances")
                        {
                            entity.FieldInstances = FieldInstance.LoadFields(property);
                        }
                        else if (property.Name == "height")
                        {
                            entity.Height = property.Value.GetInt32();
                        }
                        else if (property.Name == "px")
                        {
                            entity.Coordinates = new Vector2(property.Value.EnumerateArray().ToArray()[0].GetInt32(), property.Value.EnumerateArray().ToArray()[1].GetInt32());
                        }
                        else if (property.Name == "width")
                        {
                            entity.Width = property.Value.GetInt32();
                        }
                    }
                }
                output.Add(entity);
            }
            return output;
        }
    }

    /// <summary>
    /// An entity tile
    /// </summary>
    public struct EntityTile
    {
        /// <summary>
        /// An array of 4 Int values that refers to the tile in the tileset image: <c>[x, y, width, height]</c>
        /// </summary>
        public Rectangle SourceRectangle { get; set; }
        /// <summary>
        /// Tileset ID
        /// </summary>
        public int TilesetUid { get; set; }
    }

    /// <summary>
    /// Entity field instance
    /// </summary>
    public struct FieldInstance
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Type of the field, such as Int, Float, Enum, Bool, etc.
        /// </summary>
        public FieldType Type { get; set; }
        /// <summary>
        /// Raw text of the actual value of the field instance.
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Reference of the <b>Field definition</b> UID
        /// </summary>
        public int DefUid { get; set; }
        /// <summary>
        /// Name of the enum, if the field is an enum
        /// </summary>
        public string EnumName { get; set; }
        /// <summary>
        /// True if the field is an array
        /// </summary>
        public bool IsArray { get; set; }

        //public object ParseArrayValues()
        //{
        //    if (type == FieldType.Int)
        //    {
        //        List<int> wr = new List<int>();
        //        string array = value.Replace("[", "").Replace("]", "");
        //        foreach (string nb in array.Split(','))
        //        {
        //            if (int.TryParse(nb, out int nbConverted))
        //            {
        //                wr.Add(nbConverted);
        //            }
        //        }
        //        return wr;
        //    }
        //    else if (type == FieldType.Float)
        //    {
        //        List<float> wr = new List<float>();
        //        string array = value.Replace("[", "").Replace("]", "");
        //        foreach (string nb in array.Split(','))
        //        {
        //            if (float.TryParse(nb, out float nbConverted))
        //            {
        //                wr.Add(nbConverted);
        //            }
        //        }
        //        return wr;
        //    }
        //    else if (type == FieldType.Bool)
        //    {
        //        List<bool> wr = new List<bool>();
        //        string array = value.Replace("[", "").Replace("]", "");
        //        foreach (string nb in array.Split(','))
        //        {
        //            if (bool.TryParse(nb, out bool nbConverted))
        //            {
        //                wr.Add(nbConverted);
        //            }
        //        }
        //        return wr;
        //    }
        //    else if (type == FieldType.String || type == FieldType.Text || type == FieldType.Color || type == FieldType.Enum || type == FieldType.FilePath)
        //    {
        //        List<string> wr = new List<string>();
        //        string array = value.Replace("[", "").Replace("]", "");
        //        foreach (string nb in array.Split(','))
        //        {
        //            if (bool.TryParse(nb, out bool nbConverted))
        //            {
        //                wr.Add(nbConverted);
        //            }
        //        }
        //        return wr;
        //    }
        //    else (type == FieldType.Point)
        //    {

        //    }

        //    return new List<object>();
        //}

        public static List<FieldInstance> LoadFields(JsonProperty jsonProperty)
        {
            List<FieldInstance> output = new List<FieldInstance>();
            foreach (JsonElement jsonElement in jsonProperty.Value.EnumerateArray().ToArray())
            {
                FieldInstance field = new FieldInstance();
                foreach (JsonProperty property in jsonElement.EnumerateObject().ToArray())
                {
                    if (property.Value.ValueKind != JsonValueKind.Null)
                    {
                        if (property.Name == "__identifier")
                        {
                            field.Identifier = property.Value.GetString();
                        }
                        else if (property.Name == "__type")
                        {
                            if (property.Value.GetString().StartsWith("Enum"))
                            {
                                field.Type = FieldType.Enum;
                                field.IsArray = false;
                                field.EnumName = property.Value.GetString().Substring(5, property.Value.GetString().Length - 6);
                            }
                            else if (property.Value.GetString().StartsWith("LocalEnum"))
                            {
                                field.Type = FieldType.Enum;
                                field.IsArray = false;
                                field.EnumName = property.Value.GetString().Substring(("LocalEnum").Length + 1);
                            }
                            else if (property.Value.GetString().StartsWith("Array"))
                            {
                                field.IsArray = true;
                                string arrayType = property.Value.GetString().Substring(6, property.Value.GetString().Length - 7);
                                if (arrayType.StartsWith("Enum"))
                                {
                                    field.Type = FieldType.Enum;
                                    field.EnumName = arrayType.Substring(5, property.Value.GetString().Length - 1);
                                }
                                else if (arrayType.StartsWith("LocalEnum"))
                                {
                                    field.Type = FieldType.Enum;
                                    field.EnumName = arrayType.Substring(("LocalEnum").Length + 1);
                                }
                                else
                                {
                                    field.Type = (FieldType)Enum.Parse(typeof(FieldType), arrayType);
                                }
                            }
                            else
                            {
                                field.Type = (FieldType)Enum.Parse(typeof(FieldType), property.Value.GetString());
                                field.IsArray = false;
                            }
                        }
                        else if (property.Name == "__value")
                        {
                            field.Value = property.Value.GetRawText();
                        }
                        else if (property.Name == "defUid")
                        {
                            field.DefUid = property.Value.GetInt32();
                        }
                    }
                }
                output.Add(field);
            }
            return output;
        }
    }

    /// <summary>
    /// A entity field type
    /// </summary>
    public enum FieldType
    {
        Int,
        Float,
        Bool,
        String,
        Text,
        Enum,
        Color,
        Point,
        FilePath
    }

    /// <summary>
    /// A structure containing all the definitions of a project
    /// </summary>
    public class Definitions
    {
        /// <summary>
        /// All the entities definitions
        /// </summary>
        public List<EntitieDef> Entities { get; set; }
        /// <summary>
        /// All the enums defintions
        /// </summary>
        public List<EnumDef> Enums { get; set; }
        /// <summary>
        /// External enums are exactly the same as enums, except they have a externalRelPath to point to an external source file
        /// </summary>
        public List<EnumDef> ExternalEnums { get; set; }
        /// <summary>
        /// All the layers definitions
        /// </summary>
        public List<LayerDef> Layers { get; set; }
        /// <summary>
        /// All the tilesets
        /// </summary>
        public List<Tileset> Tilesets { get; set; }

        /// <summary>
        /// Load the definitions of a project
        /// </summary>
        /// <param name="jsonProperty">A json property containing the definitions</param>
        /// <returns></returns>
        public static Definitions LoadDefinitions(JsonProperty jsonProperty)
        {
            Definitions output = new Definitions();
            foreach (JsonProperty property in jsonProperty.Value.EnumerateObject().ToArray())
            {
                if (property.Value.ValueKind != JsonValueKind.Null)
                {
                    //do somethin
                    if (property.Name == "entities")
                    {
                        output.Entities = EntitieDef.LoadEntitiesDef(property);
                    }
                    else if (property.Name == "enums")
                    {
                        output.Enums = EnumDef.LoadEnumsDef(property);
                    }
                    else if (property.Name == "externalEnums")
                    {
                        output.ExternalEnums = EnumDef.LoadEnumsDef(property);
                    }
                    else if (property.Name == "layers")
                    {
                        output.Layers = LayerDef.LoadLayersDef(property);
                    }
                    else if (property.Name == "tilesets")
                    {
                        output.Tilesets = Tileset.LoadTilesets(property);
                    }
                }
            }
            return output;
        }
    }

    public class EnumDef
    {
        /// <summary>
        /// Relative path to the external file providing this Enum. Only for External enums
        /// </summary>
        public string ExternalRelPath { get; set; }
        /// <summary>
        /// Tileset UID if provided
        /// </summary>
        public int? IconTilesetUid { get; set; }
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        public int Uid { get; set; }
        /// <summary>
        /// All possible enum values, with their optional Tile infos
        /// </summary>
        public List<EnumValueDef> Values { get; set; }

        /// <summary>
        /// Load the enums definitions of project
        /// </summary>
        /// <param name="jsonProperty">A json property containing the enums defintions</param>
        /// <returns></returns>
        public static List<EnumDef> LoadEnumsDef(JsonProperty jsonProperty)
        {
            List<EnumDef> output = new List<EnumDef>();
            foreach (JsonElement jsonElement in jsonProperty.Value.EnumerateArray().ToArray())
            {
                EnumDef enumDef = new EnumDef();
                foreach (JsonProperty property in jsonElement.EnumerateObject().ToArray())
                {
                    if (property.Value.ValueKind != JsonValueKind.Null)
                    {
                        if (property.Name == "externalRelPath")
                        {
                            enumDef.ExternalRelPath = property.Value.GetString();
                        }
                        else if (property.Name == "iconTilesetUid")
                        {
                            enumDef.IconTilesetUid = property.Value.GetInt32();
                        }
                        else if (property.Name == "identifier")
                        {
                            enumDef.Identifier = property.Value.GetString();
                        }
                        else if (property.Name == "uid")
                        {
                            enumDef.Uid = property.Value.GetInt32();
                        }
                        else if (property.Name == "values")
                        {
                            enumDef.Values = EnumValueDef.LoadEnumsValuesDef(property);
                        }
                    }
                }
                output.Add(enumDef);
            }
            return output;
        }
    }

    /// <summary>
    /// An enum value, with the optional Tile infos
    /// </summary>
    public struct EnumValueDef
    {
        /// <summary>
        /// An rectangle that refers to the tile in the tileset image
        /// </summary>
        public Rectangle TileSourceRectangle { get; set; }
        /// <summary>
        /// Enum value
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The optional ID of the tile
        /// </summary>
        public int? TileId { get; set; }

        /// <summary>
        /// Load the enums values definitions of project
        /// </summary>
        /// <param name="jsonProperty">A json property containing the enums values defintions</param>
        /// <returns></returns>
        public static List<EnumValueDef> LoadEnumsValuesDef(JsonProperty jsonProperty)
        {
            List<EnumValueDef> output = new List<EnumValueDef>();
            foreach (JsonElement jsonElement in jsonProperty.Value.EnumerateArray().ToArray())
            {
                EnumValueDef enumValue = new EnumValueDef();
                foreach (JsonProperty property in jsonElement.EnumerateObject().ToArray())
                {
                    if (property.Value.ValueKind != JsonValueKind.Null)
                    {
                        if (property.Name == "__tileSrcRect")
                        {
                            Rectangle returned = new Rectangle(property.Value.EnumerateArray().ToArray()[0].GetInt32(), property.Value.EnumerateArray().ToArray()[1].GetInt32(), property.Value.EnumerateArray().ToArray()[2].GetInt32(), property.Value.EnumerateArray().ToArray()[3].GetInt32());
                        }
                        else if (property.Name == "id")
                        {
                            enumValue.Id = property.Value.GetString();
                        }
                        else if (property.Name == "tileId")
                        {
                            enumValue.TileId = property.Value.GetInt32();
                        }
                    }
                }
                output.Add(enumValue);
            }
            return output;
        }
    }

    /// <summary>
    /// A layer of project
    /// </summary>
    public struct LayerDef
    {
        /// <summary>
        /// Type of the layer (IntGrid, Entities, Tiles or AutoLayer)
        /// </summary>
        public LayerType Type { get; set; }
        /// <summary>
        /// Empty for now
        /// </summary>
        public int? AutoSourceLayerDefUid { get; set; }
        /// <summary>
        /// Reference to the Tileset UID being used by this auto-layer rules
        /// </summary>
        public int? AutoTilesetDefUid { get; set; }
        /// <summary>
        /// Opacity of the layer (0 to 1.0)
        /// </summary>
        public float DisplayOpacity { get; set; }
        /// <summary>
        /// Width and height of the grid in pixels
        /// </summary>
        public int GridSize { get; set; }
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Values for a IntGrid layer
        /// </summary>
        public List<IntGridValueDef> IntGridValues { get; set; }
        /// <summary>
        /// Offset of the layer, in pixels (IMPORTANT: this should be added to the LayerInstance optional offset)
        /// </summary>
        public Vector2 Offset { get; set; }
        /// <summary>
        /// Reference to the Tileset UID being used by this tile layer
        /// </summary>
        public int? TilesetDefUid { get; set; }
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        public int Uid { get; set; }

        /// <summary>
        /// Load the layers definitions of project
        /// </summary>
        /// <param name="jsonProperty">A json property containing the layers defintions</param>
        /// <returns></returns>
        public static List<LayerDef> LoadLayersDef(JsonProperty jsonProperty)
        {
            List<LayerDef> output = new List<LayerDef>();
            foreach (JsonElement jsonElement in jsonProperty.Value.EnumerateArray().ToArray())
            {
                LayerDef layerDef = new LayerDef();
                foreach (JsonProperty property in jsonElement.EnumerateObject().ToArray())
                {
                    if (property.Value.ValueKind != JsonValueKind.Null)
                    {
                        if (property.Name == "__type")
                        {
                            layerDef.Type = (LayerType)Enum.Parse(typeof(LayerType), property.Value.GetString());
                        }
                        else if (property.Name == "autoSourceLayerDefUid")
                        {
                            layerDef.AutoSourceLayerDefUid = property.Value.GetInt32();
                        }
                        else if (property.Name == "autoTilesetDefUid")
                        {
                            layerDef.AutoTilesetDefUid = property.Value.GetInt32();
                        }
                        else if (property.Name == "displayOpacity")
                        {
                            layerDef.DisplayOpacity = (float)property.Value.GetDouble();
                        }
                        else if (property.Name == "gridSize")
                        {
                            layerDef.GridSize = property.Value.GetInt32();
                        }
                        else if (property.Name == "identifier")
                        {
                            layerDef.Identifier = property.Value.GetString();
                        }
                        else if (property.Name == "intGridValues")
                        {
                            layerDef.IntGridValues = IntGridValueDef.LoadIntGridValuesDef(property);
                        }
                        else if (property.Name == "pxOffsetX")
                        {
                            layerDef.Offset = new Vector2(property.Value.GetInt32(), jsonElement.GetProperty("pxOffsetY").GetInt32());
                        }
                        else if (property.Name == "tilesetDefUid")
                        {
                            layerDef.TilesetDefUid = property.Value.GetInt32();
                        }
                        else if (property.Name == "uid")
                        {
                            layerDef.Uid = property.Value.GetInt32();
                        }
                    }
                }
                output.Add(layerDef);
            }
            return output;
        }
    }

    /// <summary>
    /// Type of a layer
    /// </summary>
    public enum LayerType
    {
        IntGrid,
        Entities,
        Tiles,
        AutoLayer
    }

    /// <summary>
    /// Values for a IntGrid layer
    /// </summary>
    public struct IntGridValueDef
    {
        /// <summary>
        /// Hex color "#rrggbb"
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// The IntGrid value itslef
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Load the Intgrid Values definitions of project
        /// </summary>
        /// <param name="jsonProperty">A json property containing the Intgrid Values defintions</param>
        /// <returns></returns>
        public static List<IntGridValueDef> LoadIntGridValuesDef(JsonProperty jsonProperty)
        {
            List<IntGridValueDef> output = new List<IntGridValueDef>();
            foreach (JsonElement jsonElement in jsonProperty.Value.EnumerateArray().ToArray())
            {
                IntGridValueDef intGridValue = new IntGridValueDef();
                foreach (JsonProperty property in jsonElement.EnumerateObject().ToArray())
                {
                    if (property.Value.ValueKind != JsonValueKind.Null)
                    {
                        if (property.Name == "color")
                        {
                            intGridValue.Color = new Color(
                                System.Drawing.ColorTranslator.FromHtml(property.Value.GetString()).R,
                                System.Drawing.ColorTranslator.FromHtml(property.Value.GetString()).G,
                                System.Drawing.ColorTranslator.FromHtml(property.Value.GetString()).B
                                );
                        }
                        else if (property.Name == "identifier")
                        {
                            intGridValue.Identifier = property.Value.GetString();
                        }
                        else if (property.Name == "value")
                        {
                            intGridValue.Value = property.Value.GetInt32();
                        }
                    }
                }
                output.Add(intGridValue);
            }
            return output;
        }
    }

    /// <summary>
    ///  An entity definition
    /// </summary>
    public struct EntitieDef
    {
        /// <summary>
        /// Base entity color
        /// </summary>
        public Color Color { get; set; }
        /// <summary>
        /// Pixel width
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Pixel height
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Pivot X coordinate (from 0 to 1.0)
        /// </summary>
        public Vector2 PivotCoordinates { get; set; }
        /// <summary>
        /// Tile ID used for optional tile display
        /// </summary>
        public int? TileId { get; set; }
        /// <summary>
        /// Tileset ID used for optional tile display
        /// </summary>
        public int? TilesetId { get; set; }
        /// <summary>
        /// Unique Int identifier
        /// </summary>
        public int Uid { get; set; }

        /// <summary>
        /// Load the entities definitions of project
        /// </summary>
        /// <param name="jsonProperty">A json property containing the entities defintions</param>
        /// <returns></returns>
        public static List<EntitieDef> LoadEntitiesDef(JsonProperty jsonProperty)
        {
            List<EntitieDef> output = new List<EntitieDef>();
            foreach (JsonElement jsonElement in jsonProperty.Value.EnumerateArray().ToArray())
            {
                EntitieDef entitieDef = new EntitieDef();
                foreach (JsonProperty property in jsonElement.EnumerateObject().ToArray())
                {
                    if (property.Value.ValueKind != JsonValueKind.Null)
                    {
                        if (property.Name == "color")
                        {
                            entitieDef.Color = new Color(
                                System.Drawing.ColorTranslator.FromHtml(property.Value.GetString()).R,
                                System.Drawing.ColorTranslator.FromHtml(property.Value.GetString()).G,
                                System.Drawing.ColorTranslator.FromHtml(property.Value.GetString()).B
                                );
                        }
                        else if (property.Name == "height")
                        {
                            entitieDef.Height = property.Value.GetInt32();
                        }
                        else if (property.Name == "identifier")
                        {
                            entitieDef.Identifier = property.Value.GetString();
                        }
                        else if (property.Name == "pivotX")
                        {
                            entitieDef.PivotCoordinates = new Vector2(property.Value.GetSingle(), jsonElement.GetProperty("pivotY").GetSingle());
                        }
                        else if (property.Name == "tileId")
                        {
                            entitieDef.TileId = property.Value.GetInt32();
                        }
                        else if (property.Name == "tilesetId")
                        {
                            entitieDef.TilesetId = property.Value.GetInt32();
                        }
                        else if (property.Name == "uid")
                        {
                            entitieDef.Uid = property.Value.GetInt32();
                        }
                        else if (property.Name == "width")
                        {
                            entitieDef.Width = property.Value.GetInt32();
                        }
                    }
                }
                output.Add(entitieDef);
            }
            return output;
        }
    }

    /// <summary>
    /// A tileset
    /// </summary>
    public struct Tileset
    {
        /// <summary>
        /// Unique String identifier
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Distance in pixels from image borders
        /// </summary>
        public int Padding { get; set; }
        /// <summary>
        /// Image width in pixels
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Image width in pixels
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Path to the source file, relative to the current project JSON file
        /// </summary>
        public string RelPath { get; set; }
        /// <summary>
        /// Space in pixels between all tiles
        /// </summary>
        public int Spacing { get; set; }
        /// <summary>
        /// Size of the grid, of each tile
        /// </summary>
        public int TileGridSize { get; set; }
        /// <summary>
        /// Unique Intidentifier
        /// </summary>
        public int Uid { get; set; }

        /// <summary>
        /// Load the tilesets of a project
        /// </summary>
        /// <param name="jsonProperty">A json property containing the Tilesets defintions</param>
        /// <returns></returns>
        public static List<Tileset> LoadTilesets(JsonProperty jsonProperty)
        {
            List<Tileset> output = new List<Tileset>();
            foreach (JsonElement jsonElement in jsonProperty.Value.EnumerateArray().ToArray())
            {
                Tileset tileset = new Tileset();
                foreach (JsonProperty property in jsonElement.EnumerateObject().ToArray())
                {
                    if (property.Value.ValueKind != JsonValueKind.Null)
                    {
                        if (property.Name == "identifier")
                        {
                            tileset.Identifier = property.Value.GetString();
                        }
                        else if (property.Name == "padding")
                        {
                            tileset.Padding = property.Value.GetInt32();
                        }
                        else if (property.Name == "pxHei")
                        {
                            tileset.Height = property.Value.GetInt32();
                        }
                        else if (property.Name == "pxWid")
                        {
                            tileset.Width = property.Value.GetInt32();
                        }
                        else if (property.Name == "relPath")
                        {
                            tileset.RelPath = property.Value.GetString();
                        }
                        else if (property.Name == "spacing")
                        {
                            tileset.Spacing = property.Value.GetInt32();
                        }
                        else if (property.Name == "tileGridSize")
                        {
                            tileset.TileGridSize = property.Value.GetInt32();
                        }
                        else if (property.Name == "uid")
                        {
                            tileset.Uid = property.Value.GetInt32();
                        }
                    }
                }
                output.Add(tileset);
            }
            return output;
        }
    }
}
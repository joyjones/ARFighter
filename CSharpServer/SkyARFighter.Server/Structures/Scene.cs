﻿using SkyARFighter.Common;
using SkyARFighter.Common.DataInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyARFighter.Server.Structures
{
    public class Scene : GameObject<SceneInfo>
    {
        public Scene(Game game, SceneInfo info)
            : base(info)
        {
            ParentGame = game;
        }
        public Game ParentGame
        {
            get; private set;
        }
        public SceneMarker StartupMarker
        {
            get; private set;
        }
        public IEnumerable<SceneMarker> Markers => markers.Values;
        public IEnumerable<SceneModel> Models => models.Values;
        public IEnumerable<Player> Players => players.Values;

        public override void Load()
        {
            foreach (var mi in SceneMarker.Records.Where(r => r.SceneId == Id))
            {
                var sm = new SceneMarker(this, mi);
                markers[sm.ParentMarker.Info.Name] = sm;
                if (StartupMarker == null)
                    StartupMarker = sm;
            }
            foreach (var mi in SceneModel.Records.Where(r => r.SceneId == Id))
            {
                var sm = new SceneModel(this, mi);
                models[sm.Id] = sm;
            }
        }
        public void AddPlayer(Player player)
        {
            if (player.CurScene != null)
                player.CurScene.RemovePlayer(player);
            player.CurScene = this;
            players[player.Id] = player;

            foreach (var plr in players.Values.ToArray())
            {
                if (plr.Id == player.Id)
                    continue;
                plr.Client_AddPlayer(player.Info);
            }
        }
        public void RemovePlayer(Player player)
        {
            players.Remove(player.Id);

            foreach (var plr in players.Values.ToArray())
            {
                if (plr.Id == player.Id)
                    continue;
                plr.Client_RemovePlayer(player.Id);
            }
        }
        public SceneModel GetModel(long id)
        {
            models.TryGetValue(id, out SceneModel model);
            return model;
        }
        public void AddModel(SceneModelInfo info)
        {
            var modelInfo = Model.GetDataInfo(info.ModelId);
            if (modelInfo == null || GetModel(info.Id) != null)
                return;
            info.SceneId = Id;
            var model = new SceneModel(this, info);
            model.Info.IsNew = true;
            model.Info.Save(ParentGame.DB);
            models[model.Id] = model;

            foreach (var plr in players.Values.ToArray())
            {
                if (plr.Id == info.CreatePlayerId)
                    continue;
                plr.Client_CreateSceneModel(info);
            }
        }
        public void TransformModel(long playerId, long modelId, Vector3 pos, Vector3 rotation, Vector3 scale)
        {
            var model = GetModel(modelId);
            if (model == null)
                return;
            if (pos != null)
                model.Pos = pos;
            if (rotation != null)
                model.Rotation = rotation;
            if (scale != null)
                model.Scale = scale;
            model.Info.Save(ParentGame.DB);

            foreach (var plr in players.Values.ToArray())
            {
                if (plr.Id == playerId)
                    continue;
                if (pos != null)
                    plr.Client_MoveSceneModel(playerId, modelId, pos);
                if (rotation != null)
                    plr.Client_RotateSceneModel(playerId, modelId, rotation);
                if (scale != null)
                    plr.Client_ScaleSceneModel(playerId, modelId, scale);
            }
        }
        public void DeleteModel(long playerId, long modelId)
        {
            var model = GetModel(modelId);
            if (model == null)
                return;
            model.Delete(ParentGame.DB);
            models.Remove(modelId);
            
            foreach (var plr in players.Values.ToArray())
            {
                if (plr.Id == playerId)
                    continue;
                plr.Client_DeleteSceneModel(playerId, modelId);
            }
        }

        private Dictionary<string, SceneMarker> markers = new Dictionary<string, SceneMarker>();
        private Dictionary<long, SceneModel> models = new Dictionary<long, SceneModel>();
        private Dictionary<long, Player> players = new Dictionary<long, Player>();
    }
}

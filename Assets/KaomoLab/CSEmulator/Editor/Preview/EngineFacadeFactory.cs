using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.KaomoLab.CSEmulator.Editor.Preview
{
    public class EngineFacadeFactory
    {
        readonly OptionBridge options;
        readonly CSEmulatorOscServer oscServer;
        readonly Engine.ILoggerFactory itemScriptLoggerFactory;
        readonly Engine.ILoggerFactory playerScriptLoggerFactory;

        public EngineFacadeFactory(
            OptionBridge options,
            CSEmulatorOscServer oscServer,
            Engine.ILoggerFactory itemScriptLoggerFactory,
            Engine.ILoggerFactory playerScriptLoggerFactory
        )
        {
            this.options = options;
            this.oscServer = oscServer;
            this.itemScriptLoggerFactory = itemScriptLoggerFactory;
            this.playerScriptLoggerFactory = playerScriptLoggerFactory;
        }

        public EngineFacade CreateDefault(
        )
        {
            return new EngineFacade(
                options,
                oscServer,
                itemScriptLoggerFactory,
                playerScriptLoggerFactory,
                ClusterVR.CreatorKit.Editor.Preview.Bootstrap.ItemCreator,
                ClusterVR.CreatorKit.Editor.Preview.Bootstrap.ItemDestroyer,
                ClusterVR.CreatorKit.Editor.Preview.Bootstrap.SpawnPointManager,
                ClusterVR.CreatorKit.Editor.Preview.Bootstrap.CommentScreenPresenter
            );
        }

    }
}

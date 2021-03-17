namespace ZZAutosplitter.versions
{
    internal class GameVersion1002 : GameVersion
    {
        public override string ProcessName => "main";

        public override string SigGame => "0C685A000000??00";

        public override int OffGameToPlayer => 0x7250;
        public override int OffSceneToDataset => 0x4B0;
        public override int OffResMgrToVideoMgr => 0x7110;
        public override int OffLoadSceneEnterStart => 0x446136;
        public override int OffLoadSceneEnterEnd => 0x44613E;
        public override int OffLoadSceneExitStart => 0x4462B9;
        public override int OffLoadSceneExitEnd => 0x4462BE;
    }
}
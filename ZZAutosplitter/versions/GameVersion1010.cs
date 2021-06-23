namespace ZZAutosplitter.versions
{
    internal class GameVersion1010 : GameVersion
    {
        public override string ProcessName => "zanthp";

        public override string SigGame => "3C785A000000??00";

        public override int OffGameToPlayer => 0x7320;
        public override int OffSceneToDataset => 0x4C0;
        public override int OffResMgrToVideoMgr => 0x71E0;
        public override int OffLoadSceneEnterStart => 0x4473E0;
        public override int OffLoadSceneEnterEnd => 0x4473E8;
        public override int OffLoadSceneExitStart => 0x447563;
        public override int OffLoadSceneExitEnd => 0x447568;
        public override int OffLeaveDuelStart => 0x4AF0F4;
        public override int OffLeaveDuelEnd => 0x4AF0FA;
    }
}
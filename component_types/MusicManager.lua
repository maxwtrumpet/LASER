MusicManager = {

    music = "event:/music/Theme",
    win = "event:/effects/win",
    explosion = "event:/effects/explosion_",
    buttons = {"event:/effects/button_change", "event:/effects/button_select"},
    guns = "event:/effects/gun",
    blank_vector = Vector3(0,0,0),
    parameters = {
        {1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0},
        {1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0},
        {1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0},
        {1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0},
        {1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0},
        {1, 1, 0, 0, 1, 1, 1, 1, 1, 0, 0},
        {1, 1, 0, 0, 1, 1, 1, 1, 1, 1, 0},
        {1, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    },

    _OnMusic = function (self, event)
        Debug.Log("Called!")
        Audio.SetEventParameter(self.music, event.parameter, event.value);
    end,

    StartLevel = function (self, level)
         Audio.SetEventParameter(self.music, "Bass High", self.parameters[level][1]);
         Audio.SetEventParameter(self.music, "Melody Low", self.parameters[level][2]);
         Audio.SetEventParameter(self.music, "Bb Low", self.parameters[level][3]);
         Audio.SetEventParameter(self.music, "Ab Stay", self.parameters[level][4]);
         Audio.SetEventParameter(self.music, "Drum 1", self.parameters[level][5]);
         Audio.SetEventParameter(self.music, "Eb", self.parameters[level][6]);
         Audio.SetEventParameter(self.music, "Bb High", self.parameters[level][7]);
         Audio.SetEventParameter(self.music, "F", self.parameters[level][8]);
         Audio.SetEventParameter(self.music, "Ab Resolve", self.parameters[level][9]);
         Audio.SetEventParameter(self.music, "Drum 2", self.parameters[level][10]);
         Audio.SetEventParameter(self.music, "Melody High", self.parameters[level][11]);
    end,

    Reset = function (self)
        Audio.SetEventParameter(self.music, "Ab Resolve", 0);
        Audio.SetEventParameter(self.music, "Ab Stay", 0);
        Audio.SetEventParameter(self.music, "Bass High", 1);
        Audio.SetEventParameter(self.music, "Bass Low", 0);
        Audio.SetEventParameter(self.music, "Bb High", 0);
        Audio.SetEventParameter(self.music, "Bb Low", 0);
        Audio.SetEventParameter(self.music, "Drum 1", 1);
        Audio.SetEventParameter(self.music, "Drum 2", 0);
        Audio.SetEventParameter(self.music, "Drum 3", 0);
        Audio.SetEventParameter(self.music, "Eb", 0);
        Audio.SetEventParameter(self.music, "F", 0);
        Audio.SetEventParameter(self.music, "Melody High", 0);
        Audio.SetEventParameter(self.music, "Melody Low", 0);
        Audio.SetEventParameter(self.music, "Ostinato Fast", 0);
        Audio.SetEventParameter(self.music, "Ostinato Slow", 0);
    end,

    Init = function (self)
        Audio.LoadBank("Master.strings")
        Audio.LoadBank("Master")
        Audio.LoadBank("Music")
        Audio.LoadBank("Sounds")
        Audio.PlayEvent(self.music, self.blank_vector, self.blank_vector, true)
        self:Reset()
        Event.Subscribe("Music", self, self._OnMusic)
    end,

}
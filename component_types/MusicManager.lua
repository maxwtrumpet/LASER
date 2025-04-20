MusicManager = {

    music = "event:/music/Theme",
    win = "event:/effects/win",
    explosion = "event:/effects/explosion_",
    buttons = {"event:/effects/button_change", "event:/effects/button_select"},
    gun = "event:/effects/gun",
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

    _OnLevel = function (self, event)
        Audio.SetEventParameter(self.music, "Bass High", self.parameters[event.level][1])
        Audio.SetEventParameter(self.music, "Melody Low", self.parameters[event.level][2])
        Audio.SetEventParameter(self.music, "Bb Low", self.parameters[event.level][3])
        Audio.SetEventParameter(self.music, "Ab Stay", self.parameters[event.level][4])
        Audio.SetEventParameter(self.music, "Drum 1", self.parameters[event.level][5])
        Audio.SetEventParameter(self.music, "Eb", self.parameters[event.level][6])
        Audio.SetEventParameter(self.music, "Bb High", self.parameters[event.level][7])
        Audio.SetEventParameter(self.music, "F", self.parameters[event.level][8])
        Audio.SetEventParameter(self.music, "Ab Resolve", self.parameters[event.level][9])
        Audio.SetEventParameter(self.music, "Drum 2", self.parameters[event.level][10])
        Audio.SetEventParameter(self.music, "Melody High", self.parameters[event.level][11])
    end,

    _OnMusic = function (self, event)
        Audio.SetEventParameter(self.music, event.parameter, event.value)
    end,

    _OnWin = function (self, event)
        Audio.PlayEvent(self.win, self.blank_vector, self.blank_vector, true)
    end,

    _OnLose = function (self, event)
        Audio.SetEventParameter(self.music, "Ab Resolve", 0)
        Audio.SetEventParameter(self.music, "Ab Stay", 0)
        Audio.SetEventParameter(self.music, "Bass High", 0)
        Audio.SetEventParameter(self.music, "Bass Low", 0)
        Audio.SetEventParameter(self.music, "Bb High", 0)
        Audio.SetEventParameter(self.music, "Bb Low", 1)
        Audio.SetEventParameter(self.music, "Drum 1", 0)
        Audio.SetEventParameter(self.music, "Drum 2", 0)
        Audio.SetEventParameter(self.music, "Drum 3", 0)
        Audio.SetEventParameter(self.music, "Eb", 0)
        Audio.SetEventParameter(self.music, "F", 0)
        Audio.SetEventParameter(self.music, "Melody High", 0)
        Audio.SetEventParameter(self.music, "Melody Low", 1)
        Audio.SetEventParameter(self.music, "Ostinato Fast", 0)
        Audio.SetEventParameter(self.music, "Ostinato Slow", 0)
    end,

    _OnExplosion = function (self, event)
        Audio.PlayEvent(self.explosion .. event.size, self.blank_vector, self.blank_vector, false)
    end,

    _OnGun = function (self, event)
        Audio.PlayEvent(self.gun .. event.main .. "-" .. event.random, self.blank_vector, self.blank_vector, false)
    end,

    _OnFocus = function (self, event)
        if event.initial == false then
            Audio.PlayEvent(self.buttons[1], self.blank_vector, self.blank_vector, true)
        end
    end,

    _OnPress = function (self)
        Audio.PlayEvent(self.buttons[2], self.blank_vector, self.blank_vector, true)
    end,

    Reset = function (self)
        Audio.SetEventParameter(self.music, "Ab Resolve", 0)
        Audio.SetEventParameter(self.music, "Ab Stay", 0)
        Audio.SetEventParameter(self.music, "Bass High", 1)
        Audio.SetEventParameter(self.music, "Bass Low", 0)
        Audio.SetEventParameter(self.music, "Bb High", 0)
        Audio.SetEventParameter(self.music, "Bb Low", 0)
        Audio.SetEventParameter(self.music, "Drum 1", 1)
        Audio.SetEventParameter(self.music, "Drum 2", 0)
        Audio.SetEventParameter(self.music, "Drum 3", 0)
        Audio.SetEventParameter(self.music, "Eb", 0)
        Audio.SetEventParameter(self.music, "F", 0)
        Audio.SetEventParameter(self.music, "Melody High", 0)
        Audio.SetEventParameter(self.music, "Melody Low", 0)
        Audio.SetEventParameter(self.music, "Ostinato Fast", 0)
        Audio.SetEventParameter(self.music, "Ostinato Slow", 0)
    end,

    Init = function (self)
        Audio.LoadBank("Master.strings")
        Audio.LoadBank("Master")
        Audio.LoadBank("Music")
        Audio.LoadBank("Sounds")
        Audio.PlayEvent(self.music, self.blank_vector, self.blank_vector, true)
        self:Reset()
        Event.Subscribe("Music", self, self._OnMusic)
        Event.Subscribe("ButtonFocus", self, self._OnFocus)
        Event.Subscribe("ButtonPress", self, self._OnPress)
        Event.Subscribe("Level", self, self._OnLevel)
        Event.Subscribe("Win", self, self._OnWin)
        Event.Subscribe("Gun", self, self._OnGun)
        Event.Subscribe("Explosion", self, self._OnExplosion)
        Event.Subscribe("Lose", self, self._OnLose)
    end,

}
// EntertainingIslandDX - ClassIsland 娱乐功能插件
// Copyright (C) 2026 Li-Ruiqi-Code
// Copyright (C) 2025 xxt8582753 (BSOD-MEMZ) - 原始 EntertainingIsland 项目
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Globalization;
using System.Speech.Recognition;
using System.Threading;
using EntertainingIsland.Models;

namespace EntertainingIsland.Services;

public class CatchphraseSpeechService : IDisposable
{
    private SpeechRecognitionEngine? _engine;
    private Thread? _thread;
    private readonly CatchphraseStore _store;
    private readonly Settings _settings;
    private volatile bool _disposed;

    public event Action<string>? PhraseMatched;

    public bool IsListening => _engine != null;

    public CatchphraseSpeechService(CatchphraseStore store, Settings settings)
    {
        _store = store;
        _settings = settings;
    }

    public void Start()
    {
        // 语音识别仅在 Windows 上可用（依赖 System.Speech / SAPI）
        if (!OperatingSystem.IsWindows())
        {
            Logger.Info("[语音识别] 当前平台不支持语音识别，已跳过");
            return;
        }

        if (_engine != null) return;

        _thread = new Thread(SpeechThread)
        {
            IsBackground = true,
            Name = "CatchphraseSpeech"
        };
        _thread.SetApartmentState(ApartmentState.STA);
        _thread.Start();
    }

    public void Stop()
    {
        Dispose();
    }

    private void SpeechThread()
    {
        try
        {
            var culture = new CultureInfo("zh-CN");
            _engine = new SpeechRecognitionEngine(culture);

            _engine.SetInputToDefaultAudioDevice();
            _engine.LoadGrammar(new DictationGrammar());

            _engine.SpeechRecognized += OnSpeechRecognized;
            _engine.SpeechRecognitionRejected += OnSpeechRejected;

            _engine.RecognizeAsync(RecognizeMode.Multiple);
            Logger.Info("[语音识别] 已启动 (zh-CN)");
        }
        catch (Exception ex)
        {
            Logger.Error($"[语音识别] 启动失败: {ex.Message}");
            _engine?.Dispose();
            _engine = null;
        }
    }

    private void OnSpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
    {
        if (_disposed) return;

        var text = e.Result.Text;
        var confidence = e.Result.Confidence;
        Logger.Info($"[语音识别] 识别到: \"{text}\" (置信度: {confidence:F2})");

        if (confidence < _settings.CatchphraseVoiceConfidence) return;

        CheckMatch(text);
    }

    private void OnSpeechRejected(object? sender, SpeechRecognitionRejectedEventArgs e)
    {
        // 识别被拒绝，不处理
    }

    private void CheckMatch(string recognizedText)
    {
        var normalized = recognizedText.Replace(" ", "");
        foreach (var preset in _settings.CatchphrasePresets)
        {
            var phrase = preset.Phrase;
            if (string.IsNullOrWhiteSpace(phrase)) continue;

            if (normalized.Contains(phrase, StringComparison.OrdinalIgnoreCase))
            {
                Logger.Info($"[语音识别] 匹配口头禅: \"{phrase}\"");
                _store.Add(phrase);
                PhraseMatched?.Invoke(phrase);
                return;
            }
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        try
        {
            if (_engine != null)
            {
                _engine.SpeechRecognized -= OnSpeechRecognized;
                _engine.SpeechRecognitionRejected -= OnSpeechRejected;
                _engine.RecognizeAsyncStop();
                _engine.Dispose();
                _engine = null;
            }
        }
        catch (Exception ex)
        {
            Logger.Error($"[语音识别] 停止失败: {ex.Message}");
        }
    }
}

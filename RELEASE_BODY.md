# 1.2.0.0

### 破坏性变更

- **移除全部全局热键功能**：不再注册任何 `RegisterHotKey` / X11 `XGrabKey`。
- **移除「窗外有老师警报」提醒提供方**（其唯一入口是热键）。
- **移除全局安全键**（原 `Ctrl+Shift+K`）。需要一键清屏请在 ClassIsland 自动化中使用「隐藏所有组件」行动。
- **移除口头禅快捷键**（原 `Ctrl+Shift+F1`~`F3`），新增自动化行动「口头禅 +1」作为计数入口。
- 移除未公开的本地 HTTP 通知服务（`localhost:8765`，无鉴权）。

### 新增

- 自动化行动：**口头禅 +1**（可指定口头禅文字，留空则用预设第一条）。
- 口头禅「强调提醒」重新接入为独立提醒提供方。

### 修改

- **点名器算法改为完全随机**：每次点名独立均匀抽样，移除加权爆率与保底机制，隐藏面板一并删除。
- 统一版本号：`manifest.yml` / `AssemblyInfo.cs` 均为 `1.2.0.0`。

### 清理

- 删除死代码：`_activeInstance` 字段、空的 `ShowEmphasis` 方法。
- 删除未注册的侧边栏设置页 `FortuneSettingsPage` / `CameraMonitorSettingsPage`。
- 删除废弃的 `cipx/checksums.md`（记录的仍是 `MyPlugin.cipx`）。
- 更新 OOBE 欢迎向导与 README，移除热键与警报相关描述。

<!-- CLASSISLAND_PKG_MD5 {"entertainingisland.app.cipx": "54d8108b376bf7833ee952061f7df92e"} -->

# 1.2.0.0

首个由 Li-Ruiqi-Code 维护的版本，基于 [BSOD-MEMZ/EntertainingIsland](https://github.com/BSOD-MEMZ/EntertainingIsland) 修改。

### 破坏性变更

- **移除全部全局热键功能**：不再注册任何 `RegisterHotKey` / X11 `XGrabKey`。
- **移除「窗外有老师警报」提醒提供方**（其唯一入口是热键）。
- **移除全局安全键**（原 `Ctrl+Shift+K`）。需要一键清屏请在 ClassIsland 自动化中使用「隐藏所有组件」行动。
- **移除口头禅快捷键**（原 `Ctrl+Shift+F1`~`F3`），新增自动化行动「口头禅 +1」作为计数入口。
- 移除未公开的本地 HTTP 通知服务（`localhost:8765`，无鉴权）。

### 新增

- 自动化行动：**口头禅 +1**（可指定口头禅文字，留空则用预设第一条）。
- 口头禅「强调提醒」重新接入为独立提醒提供方。
- 头像课程表设置页新增「时间显示」开关。

### 修改

- **点名器算法改为完全随机**：每次点名独立均匀抽样，移除加权爆率与保底机制，隐藏面板一并删除。
- **头像课程表显示当堂上课时间**：在当前课程头像右侧显示 `HH:mm-HH:mm`。上课时显示当前课，课间显示即将开始的下一节课；当堂课程始终高亮。
- **项目更名并改用 GPLv3 许可证**：新增 `LICENSE`，源码文件头部添加版权声明。
- 统一版本号：`manifest.yml` / `AssemblyInfo.cs` 均为 `1.2.0.0`。
- manifest 仓库地址与分支更新为 `Li-Ruiqi-Code/EntertainingIslandDX` / `main`。

### 修复

- **`package.ps1` 在 Windows PowerShell 5.1 下无法运行**：
  - 全部 `.ps1` 补上 UTF-8 BOM，避免中文按 ANSI 解析导致语法错误。
  - `Compress-Archive` 不接受 `.cipx` 扩展名，改为先打 zip 再改名。
  - ZIP 条目路径分隔符规整为正斜杠，符合规范。
- RSS 组件加载失败时只显示「加载失败」，不再暴露具体错误信息。

### 清理

- 删除死代码：`_activeInstance` 字段、空的 `ShowEmphasis` 方法。
- 删除未注册的侧边栏设置页 `FortuneSettingsPage` / `CameraMonitorSettingsPage`。
- 删除废弃的 `cipx/checksums.md`（记录的仍是 `MyPlugin.cipx`）。
- 删除原作者的 `cipx/EntertainingIslandDX.cipx` 分发包（与本项目重名易混淆）。
- 移除 README 中指向原作者网盘、个人站点与 B 站宣传片的链接。
- 更新 OOBE 欢迎向导与 README，移除热键与警报相关描述。

<!-- CLASSISLAND_PKG_MD5 {"entertainingisland.app.cipx": "54d8108b376bf7833ee952061f7df92e"} -->

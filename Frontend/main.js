const { app, ipcMain, nativeTheme, BrowserWindow, dialog } = require('electron');
const path = require('path')
const log = require('electron-log');
const fs = require('fs').promises;
const util = require('util');
const execFile = util.promisify(require('child_process').execFile);

let mainWindow;

function createWindow() {
	mainWindow = new BrowserWindow({
		width: 1000,
		height: 600,
		webPreferences: {
			worldSafeExecuteJavaScript: true,
			preload: path.join(__dirname, 'preload.js')
		},
		autoHideMenuBar: true,
	});
	//mainWindow.openDevTools({ mode: 'detach' });
	mainWindow.loadURL(`file://${__dirname}/index.html`);
	nativeTheme.themeSource = "dark";
	log.info(process.env.NODE_ENV);
	const resourcePath =
		!process.env.NODE_ENV || process.env.NODE_ENV === "production"
			? process.resourcesPath // Live Mode
			: __dirname; // Dev Mode

	ipcMain.handle('dark-mode:toggle', () => {
		if (nativeTheme.shouldUseDarkColors) {
			nativeTheme.themeSource = 'light'
		} else {
			nativeTheme.themeSource = 'dark'
		}
		return nativeTheme.shouldUseDarkColors
	})

	ipcMain.handle('npc-compile', async (event, ...args) => {
		let tempFile = path.join(resourcePath, "temp.npc");
		let tempCompiledFile = path.join(resourcePath, "temp.json");
		let compiler = path.join(resourcePath, "NPC.Runtime.exe");
		await fs.writeFile(tempFile, args[0]);
		const { stdout } = await execFile(compiler, ["compile", tempFile]);
		const compiled = await fs.readFile(tempCompiledFile, "utf8");
		return compiled;
	})

	ipcMain.handle('npc-beautify', async (event, ...args) => {
		let tempFile = path.join(resourcePath, "temp.npc");
		let tempBeautifiedFile = path.join(resourcePath, "temp.b.npc");
		let compiler = path.join(resourcePath, "NPC.Runtime.exe");
		await fs.writeFile(tempFile, args[0]);
		const { stdout } = await execFile(compiler, ["beautify", tempFile]);
		const compiled = await fs.readFile(tempBeautifiedFile, "utf8");
		return compiled;
	})

	ipcMain.handle('npc-run', async (event, ...args) => {
		let tempFile = path.join(resourcePath, "temp.npc");
		let tempRunResultFile = path.join(resourcePath, "temp.r.json");
		let compiler = path.join(resourcePath, "NPC.Runtime.exe");
		await fs.writeFile(tempFile, args[0]);
		const { stdout } = await execFile(compiler, ["run", tempFile]);
		const compiled = await fs.readFile(tempRunResultFile, "utf8");
		return compiled;
	})

	ipcMain.on("requestFileDialog", async () => {
		let filePath = await dialog.showOpenDialog({ properties: ['openFile'] });
		log.info(filePath);

		const content = await fs.readFile(filePath.filePaths[0], "utf8");
		mainWindow.webContents.send("fromMain", content);
	});

	mainWindow.on('closed', function () {
		mainWindow = null;
	});
}

app.on('ready', createWindow);

app.on('window-all-closed', function () {
	if (process.platform !== 'darwin') {
		app.quit();
	}
});

app.on('activate', function () {
	if (mainWindow === null) {
		createWindow();
	}
});

![](https://img.shields.io/badge/SGS-NR-orange) 
![](https://img.shields.io/badge/proj-AutoReport-purple) 
![](https://img.shields.io/badge/8-512BD4?logo=dotnet)
![](https://img.shields.io/badge/Anthropic-191919?logo=anthropic)
![](https://img.shields.io/badge/OpenAI-412991?logo=openai) 
![](https://img.shields.io/badge/GitHub_Copilot-555?logo=githubcopilot)
![](https://img.shields.io/badge/draw.io-555?logo=diagrams.net)
![](https://img.shields.io/badge/Markdown-555?logo=markdown)

# 📝SGS.NR.AutoReport

- [📝SGS.NR.AutoReport](#sgsnrautoreport)
- [💼專案資訊](#專案資訊)
- [💡專有名詞](#專有名詞)
- [🧩核心作業流程示意圖](#核心作業流程示意圖)
- [📊C4 Diagram](#c4-diagram)
  - [🌐Context](#context)
  - [📱Container](#container)
  - [📦Component](#component)
  - [💻Code](#code)
  - [🚀Deployment](#deployment)
- [📚Resources](#resources)

# 💼專案資訊

| Item          | Description                                                            |
| ------------- | ---------------------------------------------------------------------- |
| **CI No.**    | CI-2425                                                                |
| **CI Name**   | Enhancing Efficiency in Inspection Report Generation<br>(公證報告製作效率改善計畫) |
| **TWM No.**   | P-2413                                                                 |
| **TWM Dept.** | NR-MIN                                                                 |
| **TWM Name**  | 公證報告製作效率改善計畫                                                           |

# 💡專有名詞

| 名詞         | 英文                   | 名詞         | 英文                   | 名詞         | 英文                   |
|--------------|------------------------|--------------|------------------------|--------------|------------------------|
| 自然資源     | Natural Resources       | 檢驗員       | Inspector              | 檢驗工程師   | Inspection engineer     |
| 公證         | Certification           | 測試項目     | Test items             | 服務項目     | Service items           |
| 貨品特性     | Product characteristics | 裝貨         | Loading                | 卸貨         | Unloading               |
| 裝櫃         | Container loading       | 卸櫃         | Container unloading    | 取樣         | Sampling                |
| 備樣         | Sample preparation      | 化驗         | Testing                | 廢鐵         | Scrap iron              |
| 鋼捲         | Steel coil              | 鋼管         | Steel pipe             | 鋼筯         | Steel bar               |
| 裝船 | vessel loading | 卸船 | vessel unloading |

# 🧩核心作業流程示意圖

```cs
// 散裝，直接裝船
📦-🚛--裝船--🚢------(運送)-------🚢--卸船--🚛-📦

// 貨櫃，先裝櫃再上船
📦-🚛--裝櫃--🏗️--🚢--(運送)--🚢--🏗️--卸櫃--🚛-📦
```

四種主要表單，分別用於 `散裝` 與 `貨櫃` 兩種主要流程

- 裝船，Vessel Loading
- 卸船，Vessel Unloading
- 裝櫃，Container Loading
- 卸櫃，Container Unloading

# 📊C4 Diagram

>⚠️C4 是一種軟體架構的視覺化模型，代表四個層級，每一層都側重於不同的抽象程度，旨在幫助軟體開發團隊及其他關係人了解系統架構

## 🌐Context

![](./assets/SGS.NR.ReportDraftHelper-Context.drawio.svg)

## 📱Container

![](./assets/SGS.NR.ReportDraftHelper-Container.drawio.svg)

## 📦Component

![](./assets/SGS.NR.ReportDraftHelper-Component.drawio.svg)

## 💻Code

(暫無)

## 🚀Deployment

![](./assets/SGS.NR.ReportDraftHelper-Deployment.drawio.svg)

# 📚Resources

- [插入合併列印功能變數](https://support.microsoft.com/zh-tw/office/%E6%8F%92%E5%85%A5%E5%90%88%E4%BD%B5%E5%88%97%E5%8D%B0%E5%8A%9F%E8%83%BD%E8%AE%8A%E6%95%B8-9a1ab5e3-2d7a-420d-8d7e-7cc26f26acff) 插入變數 `Ctrl+F9` ，顯示/隱藏 `Alt+F9`
- [The C4 model](https://c4model.com/)
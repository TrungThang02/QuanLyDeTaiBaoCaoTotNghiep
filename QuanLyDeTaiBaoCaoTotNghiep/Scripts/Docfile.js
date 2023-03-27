$("#resolte-contaniner").officeToHtml({
    url: file_path,
    inputObjId: "select_file",
    pdfSetting: {
        setLang: "he",
        thumbnailViewBtn: true,
        searchBtn: true,
        nextPreviousBtn: true,
        pageNumberTxt: true,
        totalPagesLabel: true,
        zoomBtns: true,
        scaleSelector: true,
        presantationModeBtn: true,
        openFileBtn: true,
        printBtn: true,
        downloadBtn: true,
        bookmarkBtn: true,
        secondaryToolbarBtn: true,
        firstPageBtn: true,
        lastPageBtn: true,
        pageRotateCwBtn: true,
        pageRotateCcwBtn: true,
        cursorSelectTextToolbarBtn: true,
        cursorHandToolbarBtn: true
    },
    docxSetting: {
        styleMap: null,
        includeEmbeddedStyleMap: true,
        includeDefaultStyleMap: true,
        convertImage: null,
        ignoreEmptyParagraphs: false,
        idPrefix: "",
        isRtl: "auto"
    },
    pptxSetting: {
        slidesScale: "50%", //Change Slides scale by percent
        slideMode: true, /** true,false*/
        slideType: "divs2slidesjs", /*'divs2slidesjs' (default) , 'revealjs'(https://revealjs.com) */
        revealjsPath: "", /*path to js file of revealjs. default:  './revealjs/reveal.js'*/
        keyBoardShortCut: true,  /** true,false ,condition: slideMode: true*/
        mediaProcess: true, /** true,false: if true then process video and audio files */
        jsZipV2: false,
        slideModeConfig: {
            first: 1,
            nav: true, /** true,false : show or not nav buttons*/
            navTxtColor: "black", /** color */
            keyBoardShortCut: false, /** true,false ,condition: */
            showSlideNum: true, /** true,false */
            showTotalSlideNum: true, /** true,false */
            autoSlide: 1, /** false or seconds , F8 to active ,keyBoardShortCut: true */
            randomAutoSlide: false, /** true,false ,autoSlide:true */
            loop: true,  /** true,false */
            background: false, /** false or color*/
            transition: "default", /** transition type: "slid","fade","default","random" , to show transition efects :transitionTime > 0.5 */
            transitionTime: 1 /** transition time between slides in seconds */
        },
        revealjsConfig: {} /*revealjs options. see https://revealjs.com */
    },
    sheetSetting: {
        // setting for excel
        jqueryui: false,
        activeHeaderClassName: "",
        allowEmpty: true,
        autoColumnSize: true,
        autoRowSize: false,
        columns: false,
        columnSorting: true,
        contextMenu: false,
        copyable: true,
        customBorders: false,
        fixedColumnsLeft: 0,
        fixedRowsTop: 0,
        language: 'en-US',
        search: false,
        selectionMode: 'single',
        sortIndicator: false,
        readOnly: false,
        startRows: 1,
        startCols: 1,
        rowHeaders: true,
        colHeaders: true,
        width: false,
        height: false
    },
    imageSetting: {
        // setting for  images
        frame: ['100%', '100%', false],
        maxZoom: '900%',
        zoomFactor: '10%',
        mouse: true,
        keyboard: true,
        toolbar: true,
        rotateToolbar: false
    }
});
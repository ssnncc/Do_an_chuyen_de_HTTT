USE [QLVT_DATHANG]
GO
/****** Object:  Trigger [dbo].[TR_AfterDelete_CTPX]    Script Date: 08-May-20 12:28:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[TR_AfterDelete_CTPX]
	ON [dbo].[CTPX]
	AFTER DELETE
AS
BEGIN
	UPDATE [dbo].[Vattu]
	SET SOLUONGTON=SOLUONGTON+( SELECT SOLUONG FROM deleted)
	WHERE MAVT= (SELECT MAVT FROM deleted)
END

GO
/****** Object:  Trigger [dbo].[TR_AfterInsert_CTPX]    Script Date: 08-May-20 12:28:50 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[TR_AfterInsert_CTPX]
	ON [dbo].[CTPX]
	AFTER INSERT
AS
BEGIN
	UPDATE [dbo].[Vattu]
	SET SOLUONGTON=SOLUONGTON - ( SELECT SOLUONG FROM inserted)
	WHERE MAVT= (SELECT MAVT FROM inserted)
END

GO
/****** Object:  Trigger [dbo].[TR_AfterUpdate_CTPX]    Script Date: 08-May-20 12:29:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[TR_AfterUpdate_CTPX]
	ON [dbo].[CTPX]
	AFTER UPDATE
AS
BEGIN
	IF(UPDATE(SOLUONG))
	BEGIN
		UPDATE Vattu
		SET SOLUONGTON=SOLUONGTON-(SELECT SOLUONG FROM deleted)+(SELECT SOLUONG FROM inserted)
		WHERE MAVT=(SELECT MAVT FROM inserted)
	END
-- Trường hợp hiệu chỉnh field MAVT cũng làm ảnh hưởng tới số lượng tồn
	IF(UPDATE(MAVT))
	BEGIN
		UPDATE Vattu
		SET SOLUONGTON = SOLUONGTON +(SELECT SOLUONG FROM inserted)
		WHERE MAVT=(SELECT MAVT FROM inserted)
		UPDATE [dbo].[Vattu]
		SET SOLUONGTON=SOLUONGTON-( SELECT SOLUONG FROM deleted)
		WHERE MAVT= (SELECT MAVT FROM deleted)
	END
END
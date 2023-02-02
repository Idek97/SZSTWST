; ModuleID = 'obj\Release\130\android\compressed_assemblies.armeabi-v7a.ll'
source_filename = "obj\Release\130\android\compressed_assemblies.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android"


%struct.CompressedAssemblyDescriptor = type {
	i32,; uint32_t uncompressed_file_size
	i8,; bool loaded
	i8*; uint8_t* data
}

%struct.CompressedAssemblies = type {
	i32,; uint32_t count
	%struct.CompressedAssemblyDescriptor*; CompressedAssemblyDescriptor* descriptors
}
@__CompressedAssemblyDescriptor_data_0 = internal global [135168 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_1 = internal global [11264 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_2 = internal global [16384 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_3 = internal global [203776 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_4 = internal global [169472 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_5 = internal global [8704 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_6 = internal global [237568 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_7 = internal global [116224 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_8 = internal global [2615808 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_9 = internal global [122880 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_10 = internal global [79360 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_11 = internal global [519168 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_12 = internal global [948224 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_13 = internal global [15360 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_14 = internal global [365568 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_15 = internal global [1197568 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_16 = internal global [481280 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_17 = internal global [49664 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_18 = internal global [4648960 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_19 = internal global [14752 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_20 = internal global [358912 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_21 = internal global [742912 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_22 = internal global [30720 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_23 = internal global [19968 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_24 = internal global [219648 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_25 = internal global [51712 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_26 = internal global [8192 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_27 = internal global [419328 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_28 = internal global [55808 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_29 = internal global [5120 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_30 = internal global [68096 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_31 = internal global [557568 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_32 = internal global [15264 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_33 = internal global [77312 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_34 = internal global [1458176 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_35 = internal global [903168 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_36 = internal global [64512 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_37 = internal global [16896 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_38 = internal global [527360 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_39 = internal global [17920 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_40 = internal global [79872 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_41 = internal global [640000 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_42 = internal global [25600 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_43 = internal global [9728 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_44 = internal global [44544 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_45 = internal global [201216 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_46 = internal global [16384 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_47 = internal global [15872 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_48 = internal global [16896 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_49 = internal global [20480 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_50 = internal global [37376 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_51 = internal global [425472 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_52 = internal global [14336 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_53 = internal global [40960 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_54 = internal global [58368 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_55 = internal global [39936 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_56 = internal global [1209344 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_57 = internal global [961536 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_58 = internal global [264096 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_59 = internal global [103424 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_60 = internal global [358400 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_61 = internal global [23480 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_62 = internal global [148384 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_63 = internal global [15264 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_64 = internal global [39328 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_65 = internal global [24992 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_66 = internal global [2136992 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_67 = internal global [27040 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_68 = internal global [318880 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_69 = internal global [12288 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_70 = internal global [42496 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_71 = internal global [2286080 x i8] zeroinitializer, align 1
@__CompressedAssemblyDescriptor_data_72 = internal global [443392 x i8] zeroinitializer, align 1


; Compressed assembly data storage
@compressed_assembly_descriptors = internal global [73 x %struct.CompressedAssemblyDescriptor] [
	; 0
	%struct.CompressedAssemblyDescriptor {
		i32 135168, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([135168 x i8], [135168 x i8]* @__CompressedAssemblyDescriptor_data_0, i32 0, i32 0); data
	}, 
	; 1
	%struct.CompressedAssemblyDescriptor {
		i32 11264, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([11264 x i8], [11264 x i8]* @__CompressedAssemblyDescriptor_data_1, i32 0, i32 0); data
	}, 
	; 2
	%struct.CompressedAssemblyDescriptor {
		i32 16384, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([16384 x i8], [16384 x i8]* @__CompressedAssemblyDescriptor_data_2, i32 0, i32 0); data
	}, 
	; 3
	%struct.CompressedAssemblyDescriptor {
		i32 203776, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([203776 x i8], [203776 x i8]* @__CompressedAssemblyDescriptor_data_3, i32 0, i32 0); data
	}, 
	; 4
	%struct.CompressedAssemblyDescriptor {
		i32 169472, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([169472 x i8], [169472 x i8]* @__CompressedAssemblyDescriptor_data_4, i32 0, i32 0); data
	}, 
	; 5
	%struct.CompressedAssemblyDescriptor {
		i32 8704, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([8704 x i8], [8704 x i8]* @__CompressedAssemblyDescriptor_data_5, i32 0, i32 0); data
	}, 
	; 6
	%struct.CompressedAssemblyDescriptor {
		i32 237568, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([237568 x i8], [237568 x i8]* @__CompressedAssemblyDescriptor_data_6, i32 0, i32 0); data
	}, 
	; 7
	%struct.CompressedAssemblyDescriptor {
		i32 116224, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([116224 x i8], [116224 x i8]* @__CompressedAssemblyDescriptor_data_7, i32 0, i32 0); data
	}, 
	; 8
	%struct.CompressedAssemblyDescriptor {
		i32 2615808, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([2615808 x i8], [2615808 x i8]* @__CompressedAssemblyDescriptor_data_8, i32 0, i32 0); data
	}, 
	; 9
	%struct.CompressedAssemblyDescriptor {
		i32 122880, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([122880 x i8], [122880 x i8]* @__CompressedAssemblyDescriptor_data_9, i32 0, i32 0); data
	}, 
	; 10
	%struct.CompressedAssemblyDescriptor {
		i32 79360, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([79360 x i8], [79360 x i8]* @__CompressedAssemblyDescriptor_data_10, i32 0, i32 0); data
	}, 
	; 11
	%struct.CompressedAssemblyDescriptor {
		i32 519168, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([519168 x i8], [519168 x i8]* @__CompressedAssemblyDescriptor_data_11, i32 0, i32 0); data
	}, 
	; 12
	%struct.CompressedAssemblyDescriptor {
		i32 948224, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([948224 x i8], [948224 x i8]* @__CompressedAssemblyDescriptor_data_12, i32 0, i32 0); data
	}, 
	; 13
	%struct.CompressedAssemblyDescriptor {
		i32 15360, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([15360 x i8], [15360 x i8]* @__CompressedAssemblyDescriptor_data_13, i32 0, i32 0); data
	}, 
	; 14
	%struct.CompressedAssemblyDescriptor {
		i32 365568, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([365568 x i8], [365568 x i8]* @__CompressedAssemblyDescriptor_data_14, i32 0, i32 0); data
	}, 
	; 15
	%struct.CompressedAssemblyDescriptor {
		i32 1197568, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([1197568 x i8], [1197568 x i8]* @__CompressedAssemblyDescriptor_data_15, i32 0, i32 0); data
	}, 
	; 16
	%struct.CompressedAssemblyDescriptor {
		i32 481280, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([481280 x i8], [481280 x i8]* @__CompressedAssemblyDescriptor_data_16, i32 0, i32 0); data
	}, 
	; 17
	%struct.CompressedAssemblyDescriptor {
		i32 49664, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([49664 x i8], [49664 x i8]* @__CompressedAssemblyDescriptor_data_17, i32 0, i32 0); data
	}, 
	; 18
	%struct.CompressedAssemblyDescriptor {
		i32 4648960, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([4648960 x i8], [4648960 x i8]* @__CompressedAssemblyDescriptor_data_18, i32 0, i32 0); data
	}, 
	; 19
	%struct.CompressedAssemblyDescriptor {
		i32 14752, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([14752 x i8], [14752 x i8]* @__CompressedAssemblyDescriptor_data_19, i32 0, i32 0); data
	}, 
	; 20
	%struct.CompressedAssemblyDescriptor {
		i32 358912, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([358912 x i8], [358912 x i8]* @__CompressedAssemblyDescriptor_data_20, i32 0, i32 0); data
	}, 
	; 21
	%struct.CompressedAssemblyDescriptor {
		i32 742912, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([742912 x i8], [742912 x i8]* @__CompressedAssemblyDescriptor_data_21, i32 0, i32 0); data
	}, 
	; 22
	%struct.CompressedAssemblyDescriptor {
		i32 30720, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([30720 x i8], [30720 x i8]* @__CompressedAssemblyDescriptor_data_22, i32 0, i32 0); data
	}, 
	; 23
	%struct.CompressedAssemblyDescriptor {
		i32 19968, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([19968 x i8], [19968 x i8]* @__CompressedAssemblyDescriptor_data_23, i32 0, i32 0); data
	}, 
	; 24
	%struct.CompressedAssemblyDescriptor {
		i32 219648, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([219648 x i8], [219648 x i8]* @__CompressedAssemblyDescriptor_data_24, i32 0, i32 0); data
	}, 
	; 25
	%struct.CompressedAssemblyDescriptor {
		i32 51712, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([51712 x i8], [51712 x i8]* @__CompressedAssemblyDescriptor_data_25, i32 0, i32 0); data
	}, 
	; 26
	%struct.CompressedAssemblyDescriptor {
		i32 8192, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([8192 x i8], [8192 x i8]* @__CompressedAssemblyDescriptor_data_26, i32 0, i32 0); data
	}, 
	; 27
	%struct.CompressedAssemblyDescriptor {
		i32 419328, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([419328 x i8], [419328 x i8]* @__CompressedAssemblyDescriptor_data_27, i32 0, i32 0); data
	}, 
	; 28
	%struct.CompressedAssemblyDescriptor {
		i32 55808, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([55808 x i8], [55808 x i8]* @__CompressedAssemblyDescriptor_data_28, i32 0, i32 0); data
	}, 
	; 29
	%struct.CompressedAssemblyDescriptor {
		i32 5120, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([5120 x i8], [5120 x i8]* @__CompressedAssemblyDescriptor_data_29, i32 0, i32 0); data
	}, 
	; 30
	%struct.CompressedAssemblyDescriptor {
		i32 68096, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([68096 x i8], [68096 x i8]* @__CompressedAssemblyDescriptor_data_30, i32 0, i32 0); data
	}, 
	; 31
	%struct.CompressedAssemblyDescriptor {
		i32 557568, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([557568 x i8], [557568 x i8]* @__CompressedAssemblyDescriptor_data_31, i32 0, i32 0); data
	}, 
	; 32
	%struct.CompressedAssemblyDescriptor {
		i32 15264, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([15264 x i8], [15264 x i8]* @__CompressedAssemblyDescriptor_data_32, i32 0, i32 0); data
	}, 
	; 33
	%struct.CompressedAssemblyDescriptor {
		i32 77312, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([77312 x i8], [77312 x i8]* @__CompressedAssemblyDescriptor_data_33, i32 0, i32 0); data
	}, 
	; 34
	%struct.CompressedAssemblyDescriptor {
		i32 1458176, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([1458176 x i8], [1458176 x i8]* @__CompressedAssemblyDescriptor_data_34, i32 0, i32 0); data
	}, 
	; 35
	%struct.CompressedAssemblyDescriptor {
		i32 903168, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([903168 x i8], [903168 x i8]* @__CompressedAssemblyDescriptor_data_35, i32 0, i32 0); data
	}, 
	; 36
	%struct.CompressedAssemblyDescriptor {
		i32 64512, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([64512 x i8], [64512 x i8]* @__CompressedAssemblyDescriptor_data_36, i32 0, i32 0); data
	}, 
	; 37
	%struct.CompressedAssemblyDescriptor {
		i32 16896, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([16896 x i8], [16896 x i8]* @__CompressedAssemblyDescriptor_data_37, i32 0, i32 0); data
	}, 
	; 38
	%struct.CompressedAssemblyDescriptor {
		i32 527360, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([527360 x i8], [527360 x i8]* @__CompressedAssemblyDescriptor_data_38, i32 0, i32 0); data
	}, 
	; 39
	%struct.CompressedAssemblyDescriptor {
		i32 17920, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([17920 x i8], [17920 x i8]* @__CompressedAssemblyDescriptor_data_39, i32 0, i32 0); data
	}, 
	; 40
	%struct.CompressedAssemblyDescriptor {
		i32 79872, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([79872 x i8], [79872 x i8]* @__CompressedAssemblyDescriptor_data_40, i32 0, i32 0); data
	}, 
	; 41
	%struct.CompressedAssemblyDescriptor {
		i32 640000, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([640000 x i8], [640000 x i8]* @__CompressedAssemblyDescriptor_data_41, i32 0, i32 0); data
	}, 
	; 42
	%struct.CompressedAssemblyDescriptor {
		i32 25600, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([25600 x i8], [25600 x i8]* @__CompressedAssemblyDescriptor_data_42, i32 0, i32 0); data
	}, 
	; 43
	%struct.CompressedAssemblyDescriptor {
		i32 9728, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([9728 x i8], [9728 x i8]* @__CompressedAssemblyDescriptor_data_43, i32 0, i32 0); data
	}, 
	; 44
	%struct.CompressedAssemblyDescriptor {
		i32 44544, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([44544 x i8], [44544 x i8]* @__CompressedAssemblyDescriptor_data_44, i32 0, i32 0); data
	}, 
	; 45
	%struct.CompressedAssemblyDescriptor {
		i32 201216, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([201216 x i8], [201216 x i8]* @__CompressedAssemblyDescriptor_data_45, i32 0, i32 0); data
	}, 
	; 46
	%struct.CompressedAssemblyDescriptor {
		i32 16384, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([16384 x i8], [16384 x i8]* @__CompressedAssemblyDescriptor_data_46, i32 0, i32 0); data
	}, 
	; 47
	%struct.CompressedAssemblyDescriptor {
		i32 15872, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([15872 x i8], [15872 x i8]* @__CompressedAssemblyDescriptor_data_47, i32 0, i32 0); data
	}, 
	; 48
	%struct.CompressedAssemblyDescriptor {
		i32 16896, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([16896 x i8], [16896 x i8]* @__CompressedAssemblyDescriptor_data_48, i32 0, i32 0); data
	}, 
	; 49
	%struct.CompressedAssemblyDescriptor {
		i32 20480, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([20480 x i8], [20480 x i8]* @__CompressedAssemblyDescriptor_data_49, i32 0, i32 0); data
	}, 
	; 50
	%struct.CompressedAssemblyDescriptor {
		i32 37376, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([37376 x i8], [37376 x i8]* @__CompressedAssemblyDescriptor_data_50, i32 0, i32 0); data
	}, 
	; 51
	%struct.CompressedAssemblyDescriptor {
		i32 425472, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([425472 x i8], [425472 x i8]* @__CompressedAssemblyDescriptor_data_51, i32 0, i32 0); data
	}, 
	; 52
	%struct.CompressedAssemblyDescriptor {
		i32 14336, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([14336 x i8], [14336 x i8]* @__CompressedAssemblyDescriptor_data_52, i32 0, i32 0); data
	}, 
	; 53
	%struct.CompressedAssemblyDescriptor {
		i32 40960, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([40960 x i8], [40960 x i8]* @__CompressedAssemblyDescriptor_data_53, i32 0, i32 0); data
	}, 
	; 54
	%struct.CompressedAssemblyDescriptor {
		i32 58368, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([58368 x i8], [58368 x i8]* @__CompressedAssemblyDescriptor_data_54, i32 0, i32 0); data
	}, 
	; 55
	%struct.CompressedAssemblyDescriptor {
		i32 39936, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([39936 x i8], [39936 x i8]* @__CompressedAssemblyDescriptor_data_55, i32 0, i32 0); data
	}, 
	; 56
	%struct.CompressedAssemblyDescriptor {
		i32 1209344, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([1209344 x i8], [1209344 x i8]* @__CompressedAssemblyDescriptor_data_56, i32 0, i32 0); data
	}, 
	; 57
	%struct.CompressedAssemblyDescriptor {
		i32 961536, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([961536 x i8], [961536 x i8]* @__CompressedAssemblyDescriptor_data_57, i32 0, i32 0); data
	}, 
	; 58
	%struct.CompressedAssemblyDescriptor {
		i32 264096, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([264096 x i8], [264096 x i8]* @__CompressedAssemblyDescriptor_data_58, i32 0, i32 0); data
	}, 
	; 59
	%struct.CompressedAssemblyDescriptor {
		i32 103424, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([103424 x i8], [103424 x i8]* @__CompressedAssemblyDescriptor_data_59, i32 0, i32 0); data
	}, 
	; 60
	%struct.CompressedAssemblyDescriptor {
		i32 358400, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([358400 x i8], [358400 x i8]* @__CompressedAssemblyDescriptor_data_60, i32 0, i32 0); data
	}, 
	; 61
	%struct.CompressedAssemblyDescriptor {
		i32 23480, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([23480 x i8], [23480 x i8]* @__CompressedAssemblyDescriptor_data_61, i32 0, i32 0); data
	}, 
	; 62
	%struct.CompressedAssemblyDescriptor {
		i32 148384, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([148384 x i8], [148384 x i8]* @__CompressedAssemblyDescriptor_data_62, i32 0, i32 0); data
	}, 
	; 63
	%struct.CompressedAssemblyDescriptor {
		i32 15264, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([15264 x i8], [15264 x i8]* @__CompressedAssemblyDescriptor_data_63, i32 0, i32 0); data
	}, 
	; 64
	%struct.CompressedAssemblyDescriptor {
		i32 39328, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([39328 x i8], [39328 x i8]* @__CompressedAssemblyDescriptor_data_64, i32 0, i32 0); data
	}, 
	; 65
	%struct.CompressedAssemblyDescriptor {
		i32 24992, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([24992 x i8], [24992 x i8]* @__CompressedAssemblyDescriptor_data_65, i32 0, i32 0); data
	}, 
	; 66
	%struct.CompressedAssemblyDescriptor {
		i32 2136992, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([2136992 x i8], [2136992 x i8]* @__CompressedAssemblyDescriptor_data_66, i32 0, i32 0); data
	}, 
	; 67
	%struct.CompressedAssemblyDescriptor {
		i32 27040, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([27040 x i8], [27040 x i8]* @__CompressedAssemblyDescriptor_data_67, i32 0, i32 0); data
	}, 
	; 68
	%struct.CompressedAssemblyDescriptor {
		i32 318880, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([318880 x i8], [318880 x i8]* @__CompressedAssemblyDescriptor_data_68, i32 0, i32 0); data
	}, 
	; 69
	%struct.CompressedAssemblyDescriptor {
		i32 12288, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([12288 x i8], [12288 x i8]* @__CompressedAssemblyDescriptor_data_69, i32 0, i32 0); data
	}, 
	; 70
	%struct.CompressedAssemblyDescriptor {
		i32 42496, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([42496 x i8], [42496 x i8]* @__CompressedAssemblyDescriptor_data_70, i32 0, i32 0); data
	}, 
	; 71
	%struct.CompressedAssemblyDescriptor {
		i32 2286080, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([2286080 x i8], [2286080 x i8]* @__CompressedAssemblyDescriptor_data_71, i32 0, i32 0); data
	}, 
	; 72
	%struct.CompressedAssemblyDescriptor {
		i32 443392, ; uncompressed_file_size
		i8 0, ; loaded
		i8* getelementptr inbounds ([443392 x i8], [443392 x i8]* @__CompressedAssemblyDescriptor_data_72, i32 0, i32 0); data
	}
], align 4; end of 'compressed_assembly_descriptors' array


; compressed_assemblies
@compressed_assemblies = local_unnamed_addr global %struct.CompressedAssemblies {
	i32 73, ; count
	%struct.CompressedAssemblyDescriptor* getelementptr inbounds ([73 x %struct.CompressedAssemblyDescriptor], [73 x %struct.CompressedAssemblyDescriptor]* @compressed_assembly_descriptors, i32 0, i32 0); descriptors
}, align 4


!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"min_enum_size", i32 4}
!3 = !{!"Xamarin.Android remotes/origin/d17-4 @ 13ba222766e8e41d419327749426023194660864"}

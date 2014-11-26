DDD 框架

DingyuehaoZiyuan.Application 
应用层作用：
1、存放DTO实体，实体命名方式结尾使用Data 如：ArticleData
2、DTO接口 采用public访问
3、DTO接口实现 采用internal 访问
4、实现Domain层，如果不是跨领域操作不建议应用层调用仓储

DingyuehaoZiyuan.Domain 
领域层 ：
1、存放表实体对象（表与表直接关联的操作）
2、领域Service，业务的主要逻辑实现
3、定义业务仓储接口

DingyuehaoZiyuan.Infrastructure 
仓储层
1、数据访问逻辑的定义
2、采用internal访问，实现业务仓储接口
3、工作单元

DingyuehaoZiyuan.Tool 
帮助类

DingyuehaoZiyuan.Win 
其他应用

DingyuehaoZiyuan Site
站点
